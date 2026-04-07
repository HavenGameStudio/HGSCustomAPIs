using System.Collections.Generic;
using HGS.Tools;
using UnityEditor.Animations;
using UnityEngine;

namespace HGS.LastBastion
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorParameterInitializer : MonoBehaviour
    {
        [HGSReadOnly]
        public AnimatorController animatorController;

        public bool destroyWhenInitialized;

        [ContextMenu("Add Params")]
        public void InitializeParameters()
        {
            AnimatorControllerParameter boolParam = new AnimatorControllerParameter();
            boolParam.type = AnimatorControllerParameterType.Bool;
            boolParam.name = "isDead";
            foreach (var parameter in AnimatorControllerParameters())
            {
                if (!ParameterExist(parameter.name))
                {
                    animatorController.AddParameter(parameter);
                }
                else
                {
                    Debug.LogWarning("Parameter " + parameter.name + " already exists");
                }
            }

            Debug.Log($"Parameter Initialized");

            if (destroyWhenInitialized)
            {
                DestroyImmediate(this);
            }
        }

        private bool ParameterExist(string name)
        {
            foreach (var param in animatorController.parameters)
            {
                if (param.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        private List<AnimatorControllerParameter> AnimatorControllerParameters()
        {
            return new List<AnimatorControllerParameter>
    {
        CreateParameter(AnimatorHelper.IdleAnimationParameter, AnimatorControllerParameterType.Bool),
        CreateParameter(AnimatorHelper.WalkingAnimationParameter, AnimatorControllerParameterType.Bool),
        CreateParameter(AnimatorHelper.RunningAnimationParameter, AnimatorControllerParameterType.Bool),
        CreateParameter(AnimatorHelper.AttackAnimatorParameter, AnimatorControllerParameterType.Trigger),
        CreateParameter(AnimatorHelper.DeadAnimatorParameter, AnimatorControllerParameterType.Trigger),
        CreateParameter(AnimatorHelper.HorizontalDirectionParameter, AnimatorControllerParameterType.Float),
        CreateParameter(AnimatorHelper.VerticalDirectionParameter, AnimatorControllerParameterType.Float),
        CreateParameter(AnimatorHelper.RandomFloatParameter, AnimatorControllerParameterType.Float),
        CreateParameter(AnimatorHelper.PerformAbilityAnimationParameter, AnimatorControllerParameterType.Trigger),
    };
        }

        private AnimatorControllerParameter CreateParameter(string name, AnimatorControllerParameterType type)
        {
            return new AnimatorControllerParameter
            {
                name = name,
                type = type
            };
        }

    }
}
