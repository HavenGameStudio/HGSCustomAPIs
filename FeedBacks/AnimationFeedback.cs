using UnityEngine;

namespace FeedBacks
{
    public class AnimationFeedback : Feedback
    {
        private enum ParameterType { Int, Float, Bool, Trigger }
        public Animator animator;

        [Header("Parameter Type")]
        [SerializeField] private ParameterType parameterType;

        [Header("Trigger")]
        [SerializeField] private string parameterName;

        public override void Play()
        {
            if (animator != null && !string.IsNullOrEmpty(parameterName))
            {
                PerformAnimation();
            }
        }

        private void PerformAnimation()
        {
            switch (parameterType)
            {
                case ParameterType.Bool:
                    animator.SetBool(parameterName, true);
                    break;
                case ParameterType.Trigger:
                    animator.SetTrigger(parameterName);
                    break;
                default:
                    break;
            }
        }
    }
}
