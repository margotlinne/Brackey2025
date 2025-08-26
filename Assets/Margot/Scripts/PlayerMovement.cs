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

        [Header("Visual")]
        [SerializeField] private Transform visual;    

        private Rigidbody2D rb;
        private Vector2 moveInput;     
        private Vector2 lastDir = Vector2.right;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f; 
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput.sqrMagnitude > 1f)
                moveInput = moveInput.normalized;
        }

        private void FixedUpdate()
        {
            Vector2 targetVel = moveInput * maxSpeed;

            Vector2 cur = rb.linearVelocity;

            float ax = Mathf.Approximately(moveInput.x, 0f) ? decel : accel;
            float ay = Mathf.Approximately(moveInput.y, 0f) ? decel : accel;

            float newVx = Mathf.MoveTowards(cur.x, targetVel.x, ax * Time.fixedDeltaTime);
            float newVy = Mathf.MoveTowards(cur.y, targetVel.y, ay * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector2(newVx, newVy);

            Vector2 vel = rb.linearVelocity;
            if (vel.sqrMagnitude > 0.0001f)
                lastDir = vel.normalized;

            if (visual != null && lastDir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg;
                visual.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
}
