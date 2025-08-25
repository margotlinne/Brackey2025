using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float accel = 20f;
        [SerializeField] private float decel = 25f;

        [Header("Axis/Visual")]
        [SerializeField] private bool lockToWorldX = true;
        [SerializeField] private Transform visual;

        private Rigidbody rb;
        private float startZ;
        private Quaternion faceRight, faceLeft;

        // New Input System �Է°� ����
        private Vector2 moveInput; // OnMove()���� ������Ʈ��
        private int dir;           // ��/�� ���� (-1,0,1)

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;

            // ȸ�� ���� + Z�� ����
            rb.constraints = RigidbodyConstraints.FreezeRotationX
                           | RigidbodyConstraints.FreezeRotationY
                           | RigidbodyConstraints.FreezeRotationZ;
            if (lockToWorldX)
                rb.constraints |= RigidbodyConstraints.FreezePositionZ;

            startZ = transform.position.z;

            faceRight = Quaternion.Euler(0f, 90f, 0f);
            faceLeft = Quaternion.Euler(0f, -90f, 0f);
        }

        // PlayerInput ������Ʈ�� ȣ�� (Move �׼ǿ� ���ε���)
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput.x < -0.1f) dir = -1;
            else if (moveInput.x > 0.1f) dir = 1;
            else dir = 0;
        }

        private void FixedUpdate()
        {
            // ��ǥ �ӵ�
            float targetVx = dir * maxSpeed;
            float rate = (dir != 0) ? accel : decel;
            float newVx = Mathf.MoveTowards(rb.linearVelocity.x, targetVx, rate * Time.fixedDeltaTime);

            float vz = lockToWorldX ? 0f : rb.linearVelocity.z;
            rb.linearVelocity = new Vector3(newVx, rb.linearVelocity.y, vz);

            // Z�� ����
            if (lockToWorldX && Mathf.Abs(transform.position.z - startZ) > 0.001f)
                rb.position = new Vector3(rb.position.x, rb.position.y, startZ);

            // �ð� �� ȸ��
            if (visual != null && dir != 0)
                visual.rotation = (dir > 0) ? faceRight : faceLeft;
        }
    }
}
