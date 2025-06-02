using UnityEngine;
using UnityEngine.Events;

namespace FeedBacks
{
    public class UnityEventFeedback : Feedback
    {
        public UnityEvent unityEvent;

        public override void Play()
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke();
            }
        }
    }
}
