using PA.Data;
using UnityEngine;

namespace PA.Engine
{
    public class StateManager : MonoBehaviour
    {
        [SerializeField] private EntityManager entityManager = null;

        public StateManager CreateEntity(int id)
        {
            this.entityManager.Create(id);
            return this;
        }

        public StateManager Apply(int entityId, ComponentInfo component)
        {
            this.entityManager.Apply(entityId, component);
            return this;
        }

        public StateManager MergeState(StateUpdateInfo data)
        {
            return this;
        }
    }
}
