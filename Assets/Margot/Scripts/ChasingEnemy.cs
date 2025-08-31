using System.Collections;
using UnityEngine;

namespace Margot
{
    public class ChasingEnemy : Enemy
    {
        [Header("Approach")]
        [SerializeField] private float stopDistance = 0f; 
        [SerializeField] private float slowDistance = 0.5f; 

        [Header("Sprites")]
        public Sprite[] up;
        public Sprite[] down;
        public Sprite[] left;
        public Sprite[] right;
        public Sprite[] upLeft;
        public Sprite[] upRight;
        public Sprite[] downLeft;
        public Sprite[] downRight;

        Sprite currentSprite1;
        Sprite currentSprite2;

        Coroutine animationCoroutine = null;


        void FixedUpdate()
        {
            if (player == null)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 toPlayer = (player.position - transform.position);
            float dist = toPlayer.magnitude;

            Vector2 dir = toPlayer.normalized;
            if (canAttack) rb.linearVelocity = dir * moveSpeed;

            UpdateSprite(dir);
        }



        protected override void Update()
        {
            base.Update();

            if (animationCoroutine == null && gameObject.activeSelf)
            {
                animationCoroutine = StartCoroutine(AnimateSprites());
            }

        }

        IEnumerator AnimateSprites()
        {
            while (true)
            {
                sr.sprite = currentSprite1;
                yield return new WaitForSeconds(0.3f);
                sr.sprite = currentSprite2;
                yield return new WaitForSeconds(0.3f);
            }            
        }

    
        protected override void OnDisable()
        {
            base.OnDisable();
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
        }


        protected override void UpdateSprite(Vector2 dir)
        {
            base.UpdateSprite(dir);

            if (sr == null) return;

            if (dir.x > 0.3f && dir.y > 0.3f)
            {
                currentSprite1 = upRight[0];
                currentSprite2 = upRight[1];
            }
            else if (dir.x < -0.3f && dir.y > 0.3f)
            {
                currentSprite1 = upLeft[0];
                currentSprite2 = upLeft[1];
            }
            else if (dir.x > 0.3f && dir.y < -0.3f)
            {
                currentSprite1 = downRight[0];
                currentSprite2 = downRight[1];
            }
            else if (dir.x < -0.3f && dir.y < -0.3f)
            {
                currentSprite1 = downLeft[0];
                currentSprite2 = downLeft[1];
            }

            else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    currentSprite1 = right[0];
                    currentSprite2 = right[1];
                }
                else
                {
                    currentSprite1 = left[0];
                    currentSprite2 = left[1];
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    currentSprite1 = up[0];
                    currentSprite2 = up[1];
                }
                else
                {
                    currentSprite1 = down[0];
                    currentSprite2 = down[1];
                }
            }
        }
    }
}
