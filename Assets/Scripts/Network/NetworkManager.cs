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
        private Stack<Action> threadUnifierStack = new Stack<Action>();
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
            this.entityManager.Get(data.assignedId);
        }

        private void OnStateUpdate(StateUpdateInfo state)
        {
            if (this.previousStateFrame < state.frame)
            {
                this.previousStateFrame = state.frame;
                foreach (var entity in state.data)
                    foreach (var component in entity.components)
                    {
                    }
            }
        }

        private void OnDiffUpdate(StateUpdateInfo diff)
        {
            if (this.previousDiffFrame < diff.frame && this.previousStateFrame < diff.frame)
            {
                this.previousDiffFrame = diff.frame;
                foreach (var entity in diff.data)
                    foreach (var component in entity.components)
                        this.entityManager.Apply(entity.id, component);
            }
        }

        private Action<T> ExecuteOnMainThread<T>(Action<T> action)
        {
            return (arg) => this.threadUnifierStack.Push(() => action(arg));
        }

        #endregion

        #region Callbacks Unity

        private void Start()
        {
            this.client = new SocketClient(this.serverUrl);
            this.client
                .On("diff", this.ExecuteOnMainThread<StateUpdateInfo>(this.OnDiffUpdate))
                .On("entityCreated", this.ExecuteOnMainThread<EntityCreatedInfo>(this.OnEntityCreated))
                .On("init", this.ExecuteOnMainThread<InitInfo>(this.OnInit))
                .On("state", this.ExecuteOnMainThread<StateUpdateInfo>(this.OnStateUpdate))
                .Connect();
        }

        private void FixedUpdate()
        {
            while (this.threadUnifierStack.Count > 0)
                try { this.threadUnifierStack.Pop()(); }
                catch (Exception e) { Debug.LogError(e.Message); }

            if (Input.GetKey(KeyCode.W)) this.client.Emit("action", 2);
            if (Input.GetKey(KeyCode.S)) this.client.Emit("action", 4);
            if (Input.GetKey(KeyCode.A)) this.client.Emit("action", 1);
            if (Input.GetKey(KeyCode.D)) this.client.Emit("action", 3);
            if (Input.GetKey(KeyCode.Space)) this.client.Emit("action", 5);
        }

        #endregion
    }
}
