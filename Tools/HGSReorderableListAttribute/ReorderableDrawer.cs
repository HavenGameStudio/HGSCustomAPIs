using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HGS.Tools
{
    [CustomPropertyDrawer(typeof(HGSReorderableAttributeAttribute))]
    public class ReorderableDrawer : PropertyDrawer
    {

        private static Dictionary<int, HGSReorderableList> lists = new Dictionary<int, HGSReorderableList>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            HGSReorderableList list = GetList(property, attribute as HGSReorderableAttributeAttribute);

            return list != null ? list.GetHeight() : EditorGUIUtility.singleLineHeight;
        }

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            HGSReorderableList list = GetList(property, attribute as HGSReorderableAttributeAttribute);

            if (list != null)
            {

                list.DoList(EditorGUI.IndentedRect(position), label);
            }
            else
            {

                GUI.Label(position, "Array must extend from ReorderableArray", EditorStyles.label);
            }
        }
#endif

        public static int GetListId(SerializedProperty property)
        {

            if (property != null)
            {

                int h1 = property.serializedObject.targetObject.GetHashCode();
                int h2 = property.propertyPath.GetHashCode();

                return (((h1 << 5) + h1) ^ h2);
            }

            return 0;
        }

        public static HGSReorderableList GetList(SerializedProperty property)
        {

            return GetList(property, null, GetListId(property));
        }

        public static HGSReorderableList GetList(SerializedProperty property, HGSReorderableAttributeAttribute attrib)
        {

            return GetList(property, attrib, GetListId(property));
        }

        public static HGSReorderableList GetList(SerializedProperty property, int id)
        {

            return GetList(property, null, id);
        }

        public static HGSReorderableList GetList(SerializedProperty property, HGSReorderableAttributeAttribute attrib, int id)
        {

            if (property == null)
            {

                return null;
            }

            HGSReorderableList list = null;
            SerializedProperty array = property.FindPropertyRelative("array");

            if (array != null && array.isArray)
            {

                if (!lists.TryGetValue(id, out list))
                {

                    if (attrib != null)
                    {

                        Texture icon = !string.IsNullOrEmpty(attrib.elementIconPath) ? AssetDatabase.GetCachedIcon(attrib.elementIconPath) : null;

                        HGSReorderableList.ElementDisplayType displayType = attrib.singleLine ? HGSReorderableList.ElementDisplayType.SingleLine : HGSReorderableList.ElementDisplayType.Auto;

                        list = new HGSReorderableList(array, attrib.add, attrib.remove, attrib.draggable, displayType, attrib.elementNameProperty, attrib.elementNameOverride, icon);
                    }
                    else
                    {

                        list = new HGSReorderableList(array, true, true, true);
                    }

                    lists.Add(id, list);
                }
                else
                {

                    list.List = array;
                }
            }

            return list;
        }
    }
}