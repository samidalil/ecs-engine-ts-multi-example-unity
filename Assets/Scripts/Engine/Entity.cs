using System.Collections;
using UnityEngine;

namespace PA.Engine
{
    public class Entity : MonoBehaviour
    {
        private Vector3 targetPosition;

        public Entity SetPosition(Vector3 position)
        {
            if (Vector3.Distance(position, this.transform.position) > 0.0001f)
                this.targetPosition = position;
            return this;
        }

        private void Update()
        {
            this.transform.position = Vector3.MoveTowards(
                this.transform.position,
                this.targetPosition,
                Vector3.Distance(this.transform.position, this.targetPosition)
            );
        }
    }
}
