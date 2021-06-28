using Unity.Collections;
using Unity.Jobs;
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
            NativeArray<Ray> rayData = new NativeArray<Ray>(numRays, Allocator.TempJob);
            RayParallel parallel = new RayParallel(rayData, meshFilter.sharedMesh);

            JobHandle parallelHandle = parallel.Schedule(rayData.Length, 1);
            parallelHandle.Complete();

            Ray[] rays = rayData.ToArray();

            rayData.Dispose();

            foreach (Ray ray in rays)
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue, 10f);

            /*
            Ray[] rays = new Ray[numRays];
            Mesh baseMesh = meshFilter.sharedMesh;
            for (int i = 0; i < rays.Length; i++)
            {
                int triangleIndex = Random.Range(0, baseMesh.triangles.Length / 3);

                Vector3 a = baseMesh.vertices[baseMesh.triangles[triangleIndex]];
                Vector3 b = baseMesh.vertices[baseMesh.triangles[triangleIndex + 1]];
                Vector3 c = baseMesh.vertices[baseMesh.triangles[triangleIndex + 2]];

                Vector3 origin = MeshUtils.RandomPointInTriangle(a, b, c);

                Vector3 na = baseMesh.normals[baseMesh.triangles[triangleIndex]];
                Vector3 nb = baseMesh.normals[baseMesh.triangles[triangleIndex + 1]];
                Vector3 nc = baseMesh.normals[baseMesh.triangles[triangleIndex + 2]];

                Vector3 direction = Vector3.Cross(na - nb, nc - nb).normalized;

                rays[i] = new Ray(origin, direction);
            }

            foreach (Ray ray in rays)
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue, 10f);
             */
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
