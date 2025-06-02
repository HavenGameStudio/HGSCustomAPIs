using UnityEditor;
using UnityEngine;

namespace HGS.Tools
{
    [CustomPropertyDrawer(typeof(HGSConditionAttribute))]
    public class ConditionAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HGSConditionAttribute condAttr = (HGSConditionAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(condAttr.ConditionFieldName);

            if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
            {
                bool conditionValue = conditionProperty.boolValue;
                bool shouldShow = condAttr.Negative ? !conditionValue : conditionValue;

                if (shouldShow || !condAttr.Hidden)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
                Debug.LogWarning($"[HGSCondition] Field '{condAttr.ConditionFieldName}' not found or is not a bool.");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HGSConditionAttribute condAttr = (HGSConditionAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(condAttr.ConditionFieldName);

            if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
            {
                bool conditionValue = conditionProperty.boolValue;
                bool shouldShow = condAttr.Negative ? !conditionValue : conditionValue;

                if (shouldShow || !condAttr.Hidden)
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
