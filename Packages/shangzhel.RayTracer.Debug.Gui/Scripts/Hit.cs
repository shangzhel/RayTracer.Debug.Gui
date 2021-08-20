using System;
using UnityEngine;

namespace shangzhel.RayTracer.Debug.Gui
{
    /// <summary>
    /// Represents a recorded ray.
    /// </summary>
    [Serializable]
    public class Hit
    {
        public int[] id;
        public Vector3 from;
        public Vector3 to;
    }
}
