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
        private readonly List<int> idsToSave = new List<int>();
        private readonly List<int> idsToDelete = new List<int>();

        public Entity Apply(int id, ComponentInfo component)
        {
            if (!this.entities.ContainsKey(id))
                return null;

            var entity = this.entities[id];

            switch ((Components)component.id)
            {
                case Components.Transform:
                    entity.transform.position = component.data.position;
                    entity.transform.rotation = Quaternion.Euler(component.data.rotation);
                    //entity.transform.localScale = data.scale;
                    break;
            }

            return entity;
        }

        public Entity Create(int id)
        {
            if (this.entities.ContainsKey(id))
                return this.entities[id];

            Entity entity = GameObject.Instantiate(this.entityPrefab).GetComponent<Entity>();
            this.entities.Add(id, entity);
            return entity;
        }

        public void Flush()
        {
            foreach (int id in this.entities.Keys)
                if (!this.idsToSave.Contains(id))
                {
                    this.idsToDelete.Add(id);
                    GameObject.Destroy(this.entities[id].gameObject);
                }

            foreach (int id in this.idsToDelete)
                this.entities.Remove(id);
            this.idsToDelete.Clear();
            this.idsToSave.Clear();
        }

        public Entity Get(int id)
        {
            return this.entities.ContainsKey(id) ? this.entities[id] : null;
        }

        public Entity Prepare(int id)
        {
            this.idsToSave.Add(id);
            return this.Create(id);
        }
    }
}
