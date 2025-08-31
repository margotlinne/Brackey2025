using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {

        [HideInInspector] public Vector2 moveInput;
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
            if (!player.cantMove)
            {
                moveInput = context.ReadValue<Vector2>();

                if (moveInput.sqrMagnitude > 1f)
                    moveInput = moveInput.normalized;
            }
            else moveInput = Vector2.zero;
               
        }

        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            // set velocity directly
            player.rb.linearVelocity = moveInput * player.moveSpeed;

            // update facing direction
            Vector2 vel = player.rb.linearVelocity;
            if (vel.sqrMagnitude > 0.0001f)
                lastDir = vel.normalized;
        }
    }

}
