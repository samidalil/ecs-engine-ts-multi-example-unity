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

        public Entity Create(int id)
        {
            if (this.entities.ContainsKey(id))
                return this.entities[id];

            Entity entity = GameObject.Instantiate(this.entityPrefab).GetComponent<Entity>();
            this.entities.Add(id, entity);
            return entity;
        }

        public Entity Apply(int id, ComponentInfo component)
        {
            if (!this.entities.ContainsKey(id))
                return null;

            var entity = this.entities[id];

            switch ((Components)component.id)
            {
                case Components.Transform:
                    entity.transform.position = component.data.position;
                    //entity.transform.rotation = Quaternion.Euler(data.rotation);
                    //entity.transform.localScale = data.scale;
                    break;
            }

            return entity;
        }
    }
}
