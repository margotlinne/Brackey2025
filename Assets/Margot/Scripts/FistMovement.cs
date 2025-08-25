using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    public class MouseFollower : MonoBehaviour
    {
        [Header("Center (defaults to parent)")]
        [SerializeField] private Transform center;

        [Header("Asymmetric ellipse radii (relative to player)")]
        [SerializeField] private float rightMax = 2f;
        [SerializeField] private float leftMax = 2f;
        [SerializeField] private float upMax = 1f;
        [SerializeField] private float downMax = 0.5f;

        [Header("Follow speed")]
        [Tooltip("0 = snap immediately, higher = slower follow")]
        [SerializeField] private float followSpeed = 100f;

        private Camera cam;
        private Vector2 screenPointer;
        private bool hasPointer;

        private void Awake()
        {
            cam = Camera.main;
            if (center == null) center = transform.parent;
        }

        public void OnPoint(InputAction.CallbackContext ctx)
        {
            screenPointer = ctx.ReadValue<Vector2>();
            hasPointer = true;
        }

        private void LateUpdate()
        {
            if (cam == null || center == null) return;

            Vector3 centerWorld = center.position;
            Vector3 targetWorld;

            if (hasPointer || Mouse.current != null)
            {
                Vector2 sp = hasPointer ? screenPointer : Mouse.current.position.ReadValue();
                targetWorld = ScreenToWorldOnZ(sp, centerWorld.z);
            }
            else
            {
                return;
            }

            // local offset
            Vector3 local = center.InverseTransformPoint(targetWorld);
            local.z = 0f;

            Vector3 clamped = ClampIntoAsymmetricEllipse(local, rightMax, leftMax, upMax, downMax);

            // smooth move
            Vector3 curLocal = transform.localPosition;
            Vector3 dstLocal = clamped;

            if (followSpeed <= 0f)
                transform.localPosition = dstLocal;
            else
                transform.localPosition = Vector3.MoveTowards(curLocal, dstLocal, followSpeed * Time.deltaTime);

            var lp = transform.localPosition;
            lp.z = 0f;
            transform.localPosition = lp;
        }

        private Vector3 ScreenToWorldOnZ(Vector2 screenPos, float zPlane)
        {
            Ray ray = cam.ScreenPointToRay(screenPos);
            Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, zPlane));
            if (plane.Raycast(ray, out float dist))
                return ray.GetPoint(dist);

            Vector3 fallback = cam.ScreenToWorldPoint(
                new Vector3(screenPos.x, screenPos.y, Mathf.Abs(cam.transform.position.z - zPlane))
            );
            fallback.z = zPlane;
            return fallback;
        }

        private static Vector3 ClampIntoAsymmetricEllipse(
            Vector3 offset, float rightMax, float leftMax, float upMax, float downMax)
        {
            float a = offset.x >= 0f ? rightMax : leftMax;
            float b = offset.y >= 0f ? upMax : downMax;

            if (a <= 0f || b <= 0f) return Vector3.zero;

            float nx = offset.x / a;
            float ny = offset.y / b;

            float d = nx * nx + ny * ny;
            if (d <= 1f) return offset;

            float k = 1f / Mathf.Sqrt(d);
            return new Vector3(nx * k * a, ny * k * b, 0f);
        }
    }
}
