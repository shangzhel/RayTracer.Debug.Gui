namespace RayTracer
{
    /// <summary>
    /// Immutable structure to represent a three-dimensional vector.
    /// </summary>
    readonly struct Vector3
    {
        public readonly double x, y, z;

        /// <summary>
        /// Construct a three-dimensional vector.
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        /// <param name="z">Z component</param>
        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
