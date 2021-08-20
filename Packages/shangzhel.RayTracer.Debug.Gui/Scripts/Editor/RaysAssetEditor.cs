using UnityEditor;

namespace shangzhel.RayTracer.Debug.Gui.Editor
{
    /// <summary>
    /// Renders a simplified editor for <see cref="RaysAsset"/>s.
    /// </summary>
    /// <remarks>
    /// This stops you from expanding the <see cref="RaysAsset.hits"/> array that
    /// may contain several hundred thousand elements and unintentionally freeze
    /// the Unity Editor.
    /// </remarks>
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
