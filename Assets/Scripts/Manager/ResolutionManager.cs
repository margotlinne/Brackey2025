using UnityEngine;

namespace Margot
{
    public class ResolutionManager : MonoBehaviour
    {
        public RectTransform background;   // Background image (fixed 16:9 ratio)
        public RectTransform[] uiElements; // UI elements to be scaled
        public Transform[] worldObjects;   // World objects to be scaled

        private int baseWidth;
        private int baseHeight;

        private Vector3[] uiBaseScales;
        private Vector3[] worldBaseScales;

        private float targetAspect = 16f / 9f; // Fixed aspect ratio (16:9)

        // Uniform scale ratio (kept for other scripts to read)
        public Vector3 ScaleRatio { get; private set; } = Vector3.one;

        void Start()
        {
            baseWidth = Screen.width;
            baseHeight = Screen.height;

            // Save original scales of UI elements
            uiBaseScales = new Vector3[uiElements.Length];
            for (int i = 0; i < uiElements.Length; i++)
                uiBaseScales[i] = uiElements[i].localScale;

            // Save original scales of world objects
            worldBaseScales = new Vector3[worldObjects.Length];
            for (int i = 0; i < worldObjects.Length; i++)
                worldBaseScales[i] = worldObjects[i].localScale;
        }

        void Update()
        {
            float baseAspect = (float)baseWidth / baseHeight;
            float currentAspect = (float)Screen.width / Screen.height;

            float scale;

            if (currentAspect < targetAspect)
            {
                // Taller screen ¡æ use width ratio
                scale = (float)Screen.width / baseWidth;
            }
            else
            {
                // Wider screen ¡æ use height ratio
                scale = (float)Screen.height / baseHeight;
            }

            // Apply uniform scale (same for x and y)
            ScaleRatio = new Vector3(scale, scale, 1f);

            // Apply ratio to UI elements
            for (int i = 0; i < uiElements.Length; i++)
                uiElements[i].localScale = Vector3.Scale(uiBaseScales[i], ScaleRatio);

            // Apply ratio to world objects
            for (int i = 0; i < worldObjects.Length; i++)
                worldObjects[i].localScale = Vector3.Scale(worldBaseScales[i], ScaleRatio);
        }
    }
}
