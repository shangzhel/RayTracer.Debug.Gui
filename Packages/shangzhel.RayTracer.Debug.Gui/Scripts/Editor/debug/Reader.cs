using RayTracer;
using System.IO;
using System.Text;

namespace shangzhel.RayTracer.Debug
{
    class Reader
    {
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
