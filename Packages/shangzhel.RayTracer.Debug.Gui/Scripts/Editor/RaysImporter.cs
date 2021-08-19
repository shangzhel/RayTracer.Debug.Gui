using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

using Vector3_ = RayTracer.Vector3;

namespace shangzhel.RayTracer.Debug.Gui.Editor
{
    [ScriptedImporter(1, new string[] { "rays" })]
    class RaysImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            DebugRay[] rays;
            using (var fs = new FileStream(ctx.assetPath, FileMode.Open, FileAccess.Read))
            {
                rays = Reader.ReadFromStream(fs);
            }

            var asset = ScriptableObject.CreateInstance<RaysAsset>();

            var hits = new Hit[rays.Length];
            for (int i = 0; i < rays.Length; ++i)
            {
                hits[i] = new Hit
                {
                    id = rays[i].id,
                    from = ToVector3(rays[i].from),
                    to = ToVector3(rays[i].to)
                };
            }

            asset.hits = hits;

            ctx.AddObjectToAsset(ctx.assetPath, asset);
            ctx.SetMainObject(asset);
        }

        private static Vector3 ToVector3(Vector3_ v)
            => new Vector3((float)v.x, (float)v.y, (float)v.z);
    }
}
