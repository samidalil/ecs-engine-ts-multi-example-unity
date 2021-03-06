using PA.Data;
using PA.Engine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PA.Network
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private string serverUrl = "ws://vps-aa672c77.vps.ovh.net:8080/";
        [SerializeField] private EntityManager entityManager = null;

        private int previousDiffFrame = 0;
        private int previousStateFrame = 0;
        private Queue<System.Action> threadUnifierStack = new Queue<System.Action>();
        private SocketClient client = null;

        #region Events

        private void OnEntityCreated(EntityCreatedInfo entity)
        {
            this.entityManager.Create(entity.id);
        }

        private void OnInit(InitInfo data)
        {
            this.previousDiffFrame = data.frame;
            this.previousStateFrame = data.frame;
            this.OnStateUpdate(data);
            Entity entity = this.entityManager.Get(data.assignedId);
            Camera.main.transform.SetParent(entity.transform);
        }

        private void OnStateUpdate(StateUpdateInfo state)
        {
            if (this.previousStateFrame < state.frame)
            {
                this.previousStateFrame = state.frame;

                foreach (var entity in state.data)
                {
                    this.entityManager.Prepare(entity.id);
                    foreach (var component in entity.components)
                        this.entityManager.Apply(entity.id, component);
                }
                this.entityManager.Flush();
            }
        }

        private void OnDiffUpdate(DiffUpdateInfo diff)
        {
            if (this.previousDiffFrame < diff.frame && this.previousStateFrame < diff.frame)
            {
                this.previousDiffFrame = diff.frame;
                foreach (var entity in diff.data)
                {
                    switch (entity.eventType)
                    {
                        case NetworkEventType.CREATED:
                            var e = this.entityManager.Create(entity.id);
                            foreach (var component in entity.components)
                                this.entityManager.Apply(entity.id, component);
                            break;
                        case NetworkEventType.MODIFIED:
                            foreach (var component in entity.components)
                                this.entityManager.Apply(entity.id, component);
                            break;
                        case NetworkEventType.REMOVED:
                            this.entityManager.Remove(entity.id);
                            break;
                    }
                }
            }
        }

        private Action<T> ExecuteOnMainThread<T>(Action<T> action)
        {
            return (arg) => this.threadUnifierStack.Enqueue(() => action(arg));
        }

        #endregion

        #region Callbacks Unity

        private void Start()
        {
            this.client = new SocketClient(this.serverUrl);
            this.client
                .On("diff", this.ExecuteOnMainThread<DiffUpdateInfo>(this.OnDiffUpdate))
                .On("entityCreated", this.ExecuteOnMainThread<EntityCreatedInfo>(this.OnEntityCreated))
                .On("init", this.ExecuteOnMainThread<InitInfo>(this.OnInit))
                .On("state", this.ExecuteOnMainThread<StateUpdateInfo>(this.OnStateUpdate))
                .Connect();
        }

        private void FixedUpdate()
        {
            while (this.threadUnifierStack.Count > 0)
                try { this.threadUnifierStack.Dequeue()(); }
                catch (Exception e) { Debug.LogError(e.ToString()); }

            if (Input.GetKey(KeyCode.UpArrow)) this.client.Emit("action", Data.Action.MOVE_FORWARD);
            if (Input.GetKey(KeyCode.DownArrow)) this.client.Emit("action", Data.Action.MOVE_BACKWARD);
            if (Input.GetKey(KeyCode.LeftArrow)) this.client.Emit("action", Data.Action.MOVE_LEFT);
            if (Input.GetKey(KeyCode.RightArrow)) this.client.Emit("action", Data.Action.MOVE_RIGHT);
            if (Input.GetKeyDown(KeyCode.Space)) this.client.Emit("action", Data.Action.JUMP);
        }

        private void OnApplicationQuit()
        {
            this.client.Close();
        }

        #endregion
    }
}
