using System.Collections;
using UnityEngine;

namespace FeedBacks
{
    public class CameraShakeFeedback : Feedback
    {
        public Camera targetCamera;
        public float duration = 0.5f;
        public float magnitude = 0.1f;

        public override void Play()
        {
            if (targetCamera != null)
            {
                targetCamera.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(Shake());
            }
        }

        private IEnumerator Shake()
        {
            Vector3 originalPos = targetCamera.transform.localPosition;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                targetCamera.transform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            targetCamera.transform.localPosition = originalPos;
        }
    }
}
