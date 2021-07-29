using UnityEngine;

namespace PA.Data
{
    public class Vector
    {
        public float x;
        public float y;
        public float z;

        public static implicit operator Vector3(Vector v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
    }

    public class TransformComponent
    {
        public Vector position;
        public Vector rotation;
        public Vector scale;
    }

    /// <summary>
    /// Contient tous les champs possibles des composants
    /// </summary>
    public class MixedFieldsComponent
    {
        public Vector position;
        public Vector rotation;
        public Vector scale;
    }
}
