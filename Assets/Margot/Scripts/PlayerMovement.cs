using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private Transform visual;

        private Vector2 moveInput;
        private Vector2 lastDir = Vector2.right;

        Player player;


        private void Awake()
        {
            player = GetComponent<Player>();

            player.rb = GetComponent<Rigidbody2D>();
            player.rb.gravityScale = 0f;
            player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput.sqrMagnitude > 1f)
                moveInput = moveInput.normalized;
        }

        private void FixedUpdate()
        {
            // set velocity directly
            player.rb.linearVelocity = moveInput * player.moveSpeed;

            // update facing direction
            Vector2 vel = player.rb.linearVelocity;
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
