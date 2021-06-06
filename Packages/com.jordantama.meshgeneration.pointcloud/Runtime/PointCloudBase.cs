using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PointCloud
{
    public abstract class PointCloudBase : MonoBehaviour
    {
        // Parameters
        [SerializeField] protected int numRays;
        [SerializeField] protected bool useSeed;
        [SerializeField] protected int seed;
        [SerializeField] protected float distance;
        [SerializeField] protected LayerMask cullingMask;

        // Member variables
        [SerializeField] protected MeshRenderer meshRenderer;
        [SerializeField] protected MeshFilter meshFilter;
        
        [SerializeField] protected Mesh mesh;

        
        public bool CanExecute => meshFilter && meshFilter.sharedMesh;
        public bool CanSave => CanExecute && mesh;

        
        protected abstract void Initialise();
        protected abstract void Clear();
        public abstract void Execute();
        public abstract void Save();


        protected struct OriginJob : IJob
        {
            public Mesh Mesh;
            
            public void Execute()
            {
                throw new NotImplementedException();
            }
        }
        
        protected struct OriginParallel : IJobParallelFor
        {
            public Mesh Mesh;
            public NativeArray<Vector3> Origins;
            

            public void Execute(int index)
            {
                throw new NotImplementedException();
            }
        }
        
        protected struct RaycastParallel : IJobParallelFor
        {
            public void Execute(int index)
            {
                throw new NotImplementedException();
            }
        }
    }
}