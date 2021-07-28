using PA.Data;
using PA.Engine;
using UnityEngine;

namespace PA.Network
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private StateManager stateManager = null;

        private int previousDiffFrame = 0;
        private int previousStateFrame = 0;
        private SocketClient client = null;

        private void Start()
        {
            this.client = new SocketClient();
            this.client
                .On<EntityCreatedInfo>("entityCreated", this.OnEntityCreated)
                .On<StateUpdateInfo>("state", this.OnStateUpdate)
                .On<StateUpdateInfo>("diff", this.OnDiffUpdate)
                .Connect();
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.W)) this.client.Emit("action", 2);
            if (Input.GetKey(KeyCode.S)) this.client.Emit("action", 4);
            if (Input.GetKey(KeyCode.A)) this.client.Emit("action", 1);
            if (Input.GetKey(KeyCode.D)) this.client.Emit("action", 3);
            if (Input.GetKey(KeyCode.Space)) this.client.Emit("action", 5);
        }

        #region Events

        private void OnEntityCreated(EntityCreatedInfo entity)
        {
            try
            {
                this.stateManager.CreateEntity(entity.id);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void OnStateUpdate(StateUpdateInfo state)
        {
            try
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
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void OnDiffUpdate(StateUpdateInfo diff)
        {
            try
            {
                if (this.previousDiffFrame < diff.frame && this.previousStateFrame < diff.frame)
                {
                    this.previousDiffFrame = diff.frame;
                    foreach (var entity in diff.data)
                        foreach (var component in entity.components)
                            this.stateManager.Apply(entity.id, component);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        #endregion
    }
}
