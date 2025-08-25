using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float accel = 20f;
        [SerializeField] private float decel = 25f;

        [Header("Axis/Visual")]
        [SerializeField] private bool lockToWorldY = true; // 2D에서는 Y축 고정 여부로 바꿔줌
        [SerializeField] private Transform visual;

        private Rigidbody2D rb;
        private Quaternion faceRight, faceLeft;

        private Vector2 moveInput;
        private int dir;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 1f; // 2D에서는 useGravity 대신 gravityScale

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (lockToWorldY)
                rb.constraints |= RigidbodyConstraints2D.FreezePositionY;

            faceRight = Quaternion.Euler(0f, 0f, 0f);
            faceLeft = Quaternion.Euler(0f, 180f, 0f);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput.x < -0.1f) dir = -1;
            else if (moveInput.x > 0.1f) dir = 1;
            else dir = 0;
        }

        private void FixedUpdate()
        {
            float targetVx = dir * maxSpeed;
            float rate = (dir != 0) ? accel : decel;
            float newVx = Mathf.MoveTowards(rb.linearVelocity.x, targetVx, rate * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector2(newVx, rb.linearVelocity.y);

            if (visual != null && dir != 0)
                visual.rotation = (dir > 0) ? faceRight : faceLeft;
        }
    }
}
