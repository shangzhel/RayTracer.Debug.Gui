using RayTracer;
using System;
using SDebug = System.Diagnostics.Debug;

namespace shangzhel.RayTracer.Debug
{
    /// <summary>
    /// Represents a traced ray to be recorded.
    /// </summary>
    /// <remarks>
    /// Rays are identified by an array of <see cref="int"/>s.
    /// </remarks>
    class DebugRay : IComparable<DebugRay>
    {
        public readonly int[] id;
        public readonly Vector3 from;
        public readonly Vector3 to;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugRay"/> class.
        /// </summary>
        /// <param name="id">An identifier for the ray.</param>
        /// <param name="from">The origin of the ray.</param>
        /// <param name="to">The end point of the ray.</param>
        /// <remarks>
        /// Do not reuse or modify the <paramref name="id"/> array after recording,
        /// because the function does not maintain a separate copy of the array.
        /// </remarks>
        public DebugRay(int[] id, Vector3 from, Vector3 to)
        {
            SDebug.Assert(id.Length <= byte.MaxValue);

            this.id = id;
            this.from = from;
            this.to = to;
        }

        public int CompareTo(DebugRay other)
        {
            for (int i = 0; i < id.Length; ++i)
            {
                if (i >= other.id.Length)
                    return 1;

                if (id[i] < other.id[i])
                    return -1;
                else if (id[i] > other.id[i])
                    return 1;
            }

            if (id.Length < other.id.Length)
                return -1;

            return 0;
        }
    }
}
