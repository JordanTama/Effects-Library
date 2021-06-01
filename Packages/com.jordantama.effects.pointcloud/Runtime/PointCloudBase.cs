using UnityEngine;

namespace PointCloud
{
    public abstract class PointCloudBase : MonoBehaviour
    {
        [SerializeField] protected int numRays;
        [SerializeField] protected LayerMask cullingMask;

        [SerializeField, HideInInspector] protected MeshRenderer meshRenderer;
        [SerializeField, HideInInspector] protected MeshFilter meshFilter;
        
        [SerializeField, HideInInspector] protected Mesh mesh;

        public bool CanExecute => meshFilter && meshFilter.sharedMesh;
        public bool CanSave => CanExecute && mesh;

        protected abstract void Initialise();
        public abstract void Execute();
        public abstract void Save();

        protected abstract void Clear();
    }
}