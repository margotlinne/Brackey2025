using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    public class OrbitAimFollower2D : MonoBehaviour
    {
        [Header("Circle Orbit")]
        [SerializeField] private float radius = 1.5f;

        [Header("Look")]
        [Tooltip("Sprite forward direction offset in degrees. " +
                 "If the sprite is drawn facing right, set to -90 so aiming up rotates to -90.")]
        [SerializeField] private float lookOffsetDeg = -90f;

        [Header("Smoothing (optional)")]
        [SerializeField] private float posFollowSpeed = 0f;   // 0 = snap instantly
        [SerializeField] private float rotFollowSpeed = 0f;   // 0 = snap instantly

        [Header("Sprite Renderer (for flipping)")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Camera cam;
        private Vector2 screenPointer;
        private bool hasPointer;

        private Vector2 lastDir = Vector2.right;

        private void Awake()
        {
            cam = Camera.main;
            if (transform.parent == null)
                Debug.LogWarning("[OrbitAimFollower2D] This object should usually be a child of the player.");

            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void OnPoint(InputAction.CallbackContext ctx)
        {
            screenPointer = ctx.ReadValue<Vector2>();
            hasPointer = true;
        }

        private void LateUpdate()
        {
            var parent = transform.parent;
            if (cam == null || parent == null) return;
            if (GameManager.Instance.uiManager.rouletteCanvas.activeSelf) return;

            // 1) Convert screen pointer to world space
            Vector3 parentWorld = parent.position;
            Vector3 aimWorld;

            if (hasPointer || Mouse.current != null)
            {
                Vector2 sp = hasPointer ? screenPointer : Mouse.current.position.ReadValue();
                aimWorld = ScreenToWorldOnZ(sp, parentWorld.z);
            }
            else
            {
                return;
            }

            // 2) Compute aim direction in parent local space
            Vector3 aimLocal = parent.InverseTransformPoint(aimWorld);
            Vector2 dir = new Vector2(aimLocal.x, aimLocal.y);

            if (dir.sqrMagnitude > 1e-6f)
                lastDir = dir.normalized;

            // 3) Target position on circle
            Vector3 targetLocalPos = (Vector3)(lastDir * radius);
            targetLocalPos.z = 0f;

            if (posFollowSpeed <= 0f)
                transform.localPosition = targetLocalPos;
            else
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition, targetLocalPos, posFollowSpeed * Time.deltaTime);

            // 4) Rotation
            float targetAngle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg + lookOffsetDeg;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, targetAngle);

            if (rotFollowSpeed <= 0f)
                transform.localRotation = targetRot;
            else
                transform.localRotation = Quaternion.RotateTowards(
                    transform.localRotation, targetRot, rotFollowSpeed * Time.deltaTime);

            // 5) Sprite flipping based on aim direction
            if (spriteRenderer != null)
            {
                // Check local aim direction (relative to parent)
                spriteRenderer.flipY = lastDir.x > 0f;
            }
        }

        private Vector3 ScreenToWorldOnZ(Vector2 screenPos, float zPlane)
        {
            Ray ray = cam.ScreenPointToRay(screenPos);
            Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, zPlane));
            if (plane.Raycast(ray, out float dist))
                return ray.GetPoint(dist);

            Vector3 p = cam.ScreenToWorldPoint(
                new Vector3(screenPos.x, screenPos.y, Mathf.Abs(cam.transform.position.z - zPlane)));
            p.z = zPlane;
            return p;
        }
    }
}
