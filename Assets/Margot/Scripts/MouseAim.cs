using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Margot
{
    [RequireComponent(typeof(RectTransform))]
    public class MouseAimUI : MonoBehaviour
    {
        [Header("Clamp Area")]
        [SerializeField] private RectTransform canvasRect; // assign your Canvas (RectTransform)

        private RectTransform rectTransform;
        private Camera cam;
        private Vector2 screenPointer;
        private bool hasPointer;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            cam = Camera.main;
        }

        // Input System: bind to Pointer position action
        public void OnPoint(InputAction.CallbackContext ctx)
        {
            screenPointer = ctx.ReadValue<Vector2>();
            hasPointer = true;
        }

        private void LateUpdate()
        {
            if (canvasRect == null || cam == null) return;
            if (GameManager.Instance.uiManager.rouletteCanvas.activeSelf) return;

            // get screen point (from input or fallback to Mouse.current)
            Vector2 sp = hasPointer
                ? screenPointer
                : (Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero);

            // convert to local point inside canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, sp, cam, out var localPoint);

            // clamp within canvas rect
            Rect rect = canvasRect.rect;
            localPoint.x = Mathf.Clamp(localPoint.x, rect.xMin, rect.xMax);
            localPoint.y = Mathf.Clamp(localPoint.y, rect.yMin, rect.yMax);

            // SNAP instantly (no smoothing)
            rectTransform.anchoredPosition = localPoint;
        }
    }
}
