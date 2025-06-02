using UnityEngine;

namespace FeedBacks
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundFeedback : Feedback
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public AudioClip clip;

        public override void Play()
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
