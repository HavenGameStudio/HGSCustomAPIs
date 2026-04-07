using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace HGS.LastBastion
{
    [CustomEditor(typeof(AnimatorParameterInitializer), true)]
    [CanEditMultipleObjects]
    public class AnimatorParameterInitializerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Initalizes the parameters from the AnimatorHelper", MessageType.Info);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Animator Parameters", EditorStyles.boldLabel);
            AnimatorParameterInitializer initializer = (AnimatorParameterInitializer)target;

            DrawDefaultInspector();

            EditorGUILayout.Space(20);
            if (initializer.animatorController == null)
            {
                Animator animator = initializer.GetComponent<Animator>();

                if (animator == null)
                {
                    EditorGUILayout.HelpBox("There is no Animator attached to this game object!. You need to add Animator component in the same gameobject", MessageType.Warning);
                }
                AnimatorController controller = animator?.runtimeAnimatorController as AnimatorController;

                if (controller == null)
                {
                    EditorGUILayout.HelpBox("There is no animator controller attached to the animator component. You need to Attach an animator controller to the animator!", MessageType.Error);
                }
                else
                {
                    initializer.animatorController = controller;
                }

            }

            if (GUILayout.Button("Initialize Animator Parameters"))
            {
                initializer.InitializeParameters();
            }
        }
    }
}
