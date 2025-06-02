using UnityEditor;
using UnityEngine;

namespace HGS.Tools
{

    [CustomPropertyDrawer(typeof(HGSEnumConditionAttribute))]
    public class ShowIfEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HGSEnumConditionAttribute showIf = (HGSEnumConditionAttribute)attribute;
            SerializedProperty enumProperty = property.serializedObject.FindProperty(showIf.EnumFieldName);

            if (enumProperty != null && enumProperty.propertyType == SerializedPropertyType.Enum)
            {
                if (enumProperty.enumNames[enumProperty.enumValueIndex] == showIf.EnumValue.ToString())
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
                Debug.LogWarning($"[ShowIfEnum] Field '{showIf.EnumFieldName}' not found or not an enum.");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HGSEnumConditionAttribute showIf = (HGSEnumConditionAttribute)attribute;
            SerializedProperty enumProperty = property.serializedObject.FindProperty(showIf.EnumFieldName);

            if (enumProperty != null && enumProperty.propertyType == SerializedPropertyType.Enum)
            {
                if (enumProperty.enumNames[enumProperty.enumValueIndex] == showIf.EnumValue.ToString())
                {
                    return EditorGUI.GetPropertyHeight(property, label, true);
                }
                else
                {
                    return 0f;
                }
            }

            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }

}