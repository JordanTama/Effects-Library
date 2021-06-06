using Unity.Collections;
using UnityEngine;

namespace PointCloud
{
    [ExecuteAlways]
    public class PointCloud : PointCloudBase
    {
        private void Awake()
        {
            Initialise();
        }

        protected override void Initialise()
        {
            TryGetComponent(out meshFilter);
            TryGetComponent(out meshRenderer);
        }

        public override void Execute()
        {
            Clear();
            
            Initialise();
            
            // TODO: Execute...
            NativeArray<Vector3> origins = new NativeArray<Vector3>(numRays, Allocator.TempJob);
            
        }

        public override void Save()
        {
            // TODO: Save...
        }

        protected override void Clear()
        {
            if (mesh)
                DestroyImmediate(mesh);
        }
    }
}