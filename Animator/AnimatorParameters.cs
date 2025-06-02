using UnityEngine;

namespace HGS.LastBastion
{
    public class AnimatorHelper
    {
        public static string WalkingAnimationParameter = "Walking";
        public static string RunningAnimationParameter = "Running";
        public static string IdleAnimationParameter = "Idle";
        public static string PerformAbilityAnimationParameter = "Perform";
        public static string HorizontalDirectionParameter = "HorizontalDirection";
        public static string VerticalDirectionParameter = "VerticalDirection";
        public static string RandomFloatParameter = "RandomFloat";
        public static string AttackAnimatorParameter = "Attack";
        public static string DeadAnimatorParameter = "Death";

        public static bool HasParameter(string paramName, Animator animator)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName)
                    return true;
            }
            return false;
        }

        public static void SetBool(Animator animator, string paramName, bool parameterValue)
        {
            if (!HasParameter(paramName, animator)) return;

            animator.SetBool(paramName, parameterValue);
        }

        public static void SetFloat(Animator animator, string paramName, float paramValue)
        {
            if (!HasParameter(paramName, animator)) return;

            animator.SetFloat(paramName, paramValue);
        }

        public static void SetTrigger(Animator animator, string paramName)
        {
            if (!HasParameter(paramName, animator)) return;

            animator.SetTrigger(paramName);
        }

    }
}
