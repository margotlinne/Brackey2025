using System.Collections;
using UnityEngine;

namespace Margot
{
    public class WorldShaking : MonoBehaviour
    {
        [Header("Targets to Shake")]
        public RectTransform[] backgroundRects;
        private Coroutine shakeRoutine;

        // Store original positions
        private Vector3[] originalPositions;

        void Awake()
        {
            // Save the starting positions of all rects
            originalPositions = new Vector3[backgroundRects.Length];
            for (int i = 0; i < backgroundRects.Length; i++)
            {
                if (backgroundRects[i] != null)
                    originalPositions[i] = backgroundRects[i].localPosition;
            }
        }

        public void Shake(float duration = 0.2f, float magnitude = 7f)
        {
            if (shakeRoutine != null)
                StopCoroutine(shakeRoutine);

            shakeRoutine = StartCoroutine(ShakeRoutine(duration, magnitude));
        }

        private IEnumerator ShakeRoutine(float duration, float magnitude)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float offsetX = Random.Range(-1f, 1f) * magnitude;
                float offsetY = Random.Range(-1f, 1f) * magnitude;

                for (int i = 0; i < backgroundRects.Length; i++)
                {
                    if (backgroundRects[i] != null)
                        backgroundRects[i].localPosition =
                            originalPositions[i] + new Vector3(offsetX, offsetY, 0f);
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Reset all rects to their original positions
            for (int i = 0; i < backgroundRects.Length; i++)
            {
                if (backgroundRects[i] != null)
                    backgroundRects[i].localPosition = originalPositions[i];
            }

            shakeRoutine = null;
        }
    }
}
