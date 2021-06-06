using UnityEngine;

namespace MeshGeneration
{
    public static class MeshUtils
    {
        public static Vector3 RandomPointInTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            float tc = Random.value;
            float tb = Random.value;

            Vector3 p = tc + tb > 1
                ? a + (1.0f - tc) * (c - a) + (1.0f - tb) * (b - a)
                : a + tc * (c - a) + tb * (b - a);

            return p;
        }
    }
}