using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;
        private PlayerMovement movement;
        private PlayerAttack attack;
        private Player player;

        void Awake()
        {
            anim = GetComponent<Animator>();
            movement = GetComponent<PlayerMovement>();
            attack = GetComponent<PlayerAttack>();
            player = GetComponent<Player>();
        }

        void Update()
        {
            if (GameManager.Instance.uiManager.isCanvasOn)
            {
                anim.speed = 0f;
                return;
            }
            anim.speed = 1f;

            Vector2 move = movement.moveInput;

            anim.SetFloat("MoveX", move.x);
            anim.SetFloat("MoveY", move.y);
            anim.SetBool("IsMoving", move.sqrMagnitude > 0.01f);

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 aimDir = (mousePos - transform.position).normalized;

            anim.SetFloat("AimX", aimDir.x);
            anim.SetFloat("AimY", aimDir.y);

            bool isShooting = attack != null && attack.canShoot && player.fireCooldown > 0f;
            anim.SetBool("IsShooting", isShooting);
        }
    }
}
