using System;
using System.Reflection.Emit;
using Photon.Deterministic;
using Quantum.Core;
using Quantum.Physics3D;

namespace Quantum
{
    public unsafe struct ShapeCastFilter
    {
        public EntityRef EntityRef;
        public ShapeCastState* State;
        public Transform3D* Transform3D;
    }

    public unsafe class ShapeCastSystem : SystemMainThreadFilter<ShapeCastFilter>
    {
        public override void Update(Frame f, ref ShapeCastFilter filter)
        {
            var position = filter.Transform3D->Position;
            var config = f.FindAsset<ShapeCastConfig>(filter.State->Config.Id);
            var shape = Shape3D.CreateSphere(config.Radius);

            //Normal Raycast - Work
            //var RaycasthHit = f.Physics3D.Raycast(position, FPVector3.Up, config.Distance, layerMask: config.AttackLayers, QueryOptions.HitAll);

            //if (RaycasthHit.HasValue)
            //{
            //  Draw.Line(position, RaycasthHit.Value.Point, color: ColorRGBA.Blue);
            //}

            //ShapeCast - Not Work - Am I missing something?
            var hits = f.Physics3D.ShapeCastAll(position, FPQuaternion.Identity, &shape, FPVector3.Up * config.Distance, layerMask: config.AttackLayers, QueryOptions.HitAll | QueryOptions.ComputeDetailedInfo);


            //var hits.SortCastDistance()

            for (int i = 0; i < hits.Count; i++)
            {
                var hit = hits.HitsBuffer[i];
              
                Log.Debug("Point:" + hit.Point);
                Log.Debug("Name: " + hit.Entity);

                //Draw.Sphere(hit.Point, FP._0_05, color: ColorRGBA.Red);
                Draw.Line(position, hit.Point, color: ColorRGBA.Blue);
            }


            Log.Debug("Count:" + hits.Count);

            Draw.Sphere(position + FPVector3.Up * config.Distance, config.Radius);
        }
    }
}