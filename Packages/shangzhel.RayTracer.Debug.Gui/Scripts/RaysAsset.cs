using UnityEngine;

namespace shangzhel.RayTracer.Debug.Gui
{
    /// <summary>
    /// Represents a collection of recorded rays.
    /// </summary>
    public class RaysAsset : ScriptableObject
    {
        public Hit[] hits;
    }
}
