using UnityEditor;
using UnityEngine;

namespace PointCloud
{
    [CustomEditor(typeof(PointCloud))]
    public class PointCloudEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (!(target is PointCloud pointCloud))
                return;
            
            DrawDefaultInspector();

            GUILayout.BeginHorizontal();
            
            GUI.enabled = pointCloud.CanExecute;
            if (GUILayout.Button("Execute"))
                pointCloud.Execute();
            GUI.enabled = true;
            
            GUI.enabled = pointCloud.CanSave;
            if (GUILayout.Button("Save"))
                pointCloud.Save();
            GUI.enabled = true;
            
            GUILayout.EndHorizontal();
        }
    }
}