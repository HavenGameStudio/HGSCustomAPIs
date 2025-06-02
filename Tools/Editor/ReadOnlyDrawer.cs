using UnityEngine;
using UnityEditor;

namespace HGS.Tools
{

    [CustomPropertyDrawer(typeof(HGSReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false; // Disable editing
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;  // Re-enable GUI so Unity doesn't remain in a disabled state
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
