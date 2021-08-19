using RayTracer;
using System;
using SDebug = System.Diagnostics.Debug;

namespace shangzhel.RayTracer.Debug
{
    class DebugRay : IComparable<DebugRay>
    {
        public readonly int[] id;
        public readonly Vector3 from;
        public readonly Vector3 to;

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
