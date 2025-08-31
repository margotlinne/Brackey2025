using UnityEngine;

namespace Margot
{
    public class ResolutionManager : MonoBehaviour
    {
        public RectTransform background;
        public RectTransform[] uiElements;
        public Transform[] worldObjects;

        private int baseWidth;
        private int baseHeight;

        private Vector2[] uiBaseSizes;
        private Vector3[] worldBaseScales;

        private float targetAspect = 16f / 9f;

        public Vector3 ScaleRatio { get; private set; } = Vector3.one;

        void Start()
        {
            baseWidth = Screen.width;
            baseHeight = Screen.height;

            uiBaseSizes = new Vector2[uiElements.Length];
            for (int i = 0; i < uiElements.Length; i++)
                uiBaseSizes[i] = uiElements[i].sizeDelta;

            worldBaseScales = new Vector3[worldObjects.Length];
            for (int i = 0; i < worldObjects.Length; i++)
                worldBaseScales[i] = worldObjects[i].localScale;
        }

        void Update()
        {
            float baseAspect = (float)baseWidth / baseHeight;
            float currentAspect = (float)Screen.width / Screen.height;

            float scale = currentAspect < targetAspect
                ? (float)Screen.width / baseWidth
                : (float)Screen.height / baseHeight;

            ScaleRatio = new Vector3(scale, scale, 1f);

            for (int i = 0; i < uiElements.Length; i++)
                uiElements[i].sizeDelta = uiBaseSizes[i] * scale;

            for (int i = 0; i < worldObjects.Length; i++)
                worldObjects[i].localScale = Vector3.Scale(worldBaseScales[i], ScaleRatio);
        }
    }
}
