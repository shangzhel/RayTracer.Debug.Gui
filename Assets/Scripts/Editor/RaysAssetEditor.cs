using UnityEditor;

namespace shangzhel.RayTracer.Debug.Gui.Editor
{
    [CustomEditor(typeof(RaysAsset))]
    class RaysAssetEditor : UnityEditor.Editor
    {
        private SerializedProperty hitsProperty;

        private void OnEnable()
        {
            hitsProperty = serializedObject.FindProperty("hits");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Entries", $"{hitsProperty.arraySize}");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
