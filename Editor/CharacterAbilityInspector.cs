using HGS.LastBastion.Character.CharacterAbilities;
using UnityEditor;
using UnityEngine;

namespace HGS.LastBastion.CustomInspector
{
    [CustomEditor(typeof(CharacterAbility), true)]
    [CanEditMultipleObjects]
    public class CharacterAbilityInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            CharacterAbility t = (target as CharacterAbility);

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            if (t.HelpBoxText() != "")
            {
                EditorGUILayout.HelpBox(t.HelpBoxText(), MessageType.Info);
            }

            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
}
