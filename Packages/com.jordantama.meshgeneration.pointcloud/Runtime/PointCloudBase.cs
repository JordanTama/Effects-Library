using System;
using MeshGeneration;
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


        protected struct RayParallel : IJobParallelFor
        {
            private NativeArray<Ray> _rays;
            private NativeArray<Vector3> _vertices;
            private NativeArray<Vector3> _normals;
            private NativeArray<int> _triangles;
            private NativeArray<int> _triangleIndices;


            public RayParallel(NativeArray<Ray> rays, Mesh mesh)
            {
                _rays = rays;

                _vertices = new NativeArray<Vector3>(mesh.vertices, Allocator.Persistent);
                _normals = new NativeArray<Vector3>(mesh.normals, Allocator.Persistent);
                _triangles = new NativeArray<int>(mesh.triangles, Allocator.Persistent);

                _triangleIndices = new NativeArray<int>(_rays.Length, Allocator.Persistent);
                for (int i = 0; i < _triangleIndices.Length; i++)
                    _triangleIndices[i] = Random.Range(0, _triangles.Length / 3);
            }


            public void Execute(int index)
            {
                int triangleIndex = _triangleIndices[index] * 3;
                Vector3 a = _vertices[_triangles[triangleIndex]];
                Vector3 b = _vertices[_triangles[triangleIndex + 1]];
                Vector3 c = _vertices[_triangles[triangleIndex + 2]];

                Vector3 origin = MeshUtils.RandomPointInTriangle(a, b, c);

                Vector3 na = _normals[_triangles[triangleIndex]];
                Vector3 nb = _normals[_triangles[triangleIndex + 1]];
                Vector3 nc = _normals[_triangles[triangleIndex + 2]];

                Vector3 direction = Vector3.Cross(na - nb, nc - nb).normalized;

                _rays[index] = new Ray(origin, direction);

            }
        }
    }
}
