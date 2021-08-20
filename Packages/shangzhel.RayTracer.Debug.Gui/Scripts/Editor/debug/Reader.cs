using RayTracer;
using System.IO;
using System.Text;

namespace shangzhel.RayTracer.Debug
{
    /// <summary>
    /// Implements a function to interpret dumped rays.
    /// </summary>
    class Reader
    {
        /// <summary>
        /// Deserializes collected rays from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>Rays collected previously.</returns>
        public static DebugRay[] ReadFromStream(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.UTF8, true);
            var arr = new DebugRay[reader.ReadInt32()];
            for (int i = 0; i < arr.Length; ++i)
            {
                var id = new int[reader.ReadByte()];
                for (int j = 0; j < id.Length; ++j)
                {
                    id[j] = reader.ReadInt32();
                }
                var from = new Vector3(reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble());
                var to = new Vector3(reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble());
                arr[i] = new DebugRay(id, from, to);
            }
            return arr;
        }
    }
}
