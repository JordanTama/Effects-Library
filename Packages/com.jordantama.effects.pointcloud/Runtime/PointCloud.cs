using System;
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
            mesh = new Mesh();
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