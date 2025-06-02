using UnityEngine;
using System.Collections.Generic;

namespace FeedBacks
{
    public class FeedbackManager : MonoBehaviour
    {
        public List<Feedback> feedbacks = new List<Feedback>();

        public void PlayAllFeedbacks()
        {
            foreach (var feedback in feedbacks)
            {
                if (feedback != null)
                {
                    feedback.Play();
                }
            }
        }

        public void PlayFeedback(int index)
        {
            if (index >= 0 && index < feedbacks.Count && feedbacks[index] != null)
            {
                feedbacks[index].Play();
            }
        }
    }
}
