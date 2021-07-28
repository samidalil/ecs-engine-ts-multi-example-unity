using System;
using UnityEngine;

namespace PA.Data
{
    public enum Components
    {
        None = 0,
        Time = 1 << 0,
        Transform = 1 << 1,
        Physics = 1 << 2,
        Action = 1 << 3,
        Network = 1 << 4,
    }

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
