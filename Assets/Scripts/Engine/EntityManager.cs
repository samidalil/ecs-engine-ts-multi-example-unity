using PA.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PA.Engine
{
    public class EntityManager : MonoBehaviour
    {
        [SerializeField] private GameObject entityPrefab = null;

        private readonly Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        private readonly Stack<int> entitiesToCreate = new Stack<int>();
        private readonly Stack<Action> actionsToLaunch = new Stack<Action>();

        public void Create(int id)
        {
            if (!this.entities.ContainsKey(id)) this.entitiesToCreate.Push(id);
        }

        public Entity Apply(int id, ComponentInfo component)
        {
            if (!this.entities.ContainsKey(id))
                return null;

            var entity = this.entities[id];
            try
            {
                switch ((Components)component.id)
                {
                    case Components.Transform:
                        this.actionsToLaunch.Push(() =>
                        {
                            entity.transform.position = component.data.position;
                            //entity.transform.rotation = Quaternion.Euler(data.rotation);
                            //entity.transform.localScale = data.scale;
                        });
                        break;
                }
            } catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return entity;
        }

        public void Update()
        {
            if (this.entitiesToCreate.Count > 0)
            {
                int id = this.entitiesToCreate.Pop();
                Entity entity = GameObject.Instantiate(this.entityPrefab).GetComponent<Entity>();
                this.entities.Add(id, entity);
            }
            if (this.actionsToLaunch.Count > 0)
            {
                this.actionsToLaunch.Pop()();
            }
        }
    }
}
