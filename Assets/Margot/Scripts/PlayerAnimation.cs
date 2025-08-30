using UnityEngine;

namespace Margot
{
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Sprites - walking, shooting")]
        public Sprite[] up;
        public Sprite[] down;
        public Sprite[] left;
        public Sprite[] right;
        public Sprite[] upRight;
        public Sprite[] upLeft;
        public Sprite[] downRight;
        public Sprite[] downLeft;

        SpriteRenderer sr;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            UpdateSprite(GetComponent<PlayerMovement>().moveInput);
        }

        void UpdateSprite(Vector2 dir)
        {

            if (sr == null) return;

            if (dir.x > 0.3f && dir.y > 0.3f)
            {
                sr.sprite = upRight[0];
            }
            else if (dir.x < -0.3f && dir.y > 0.3f)
            {
                sr.sprite = upLeft[0];
            }
            else if (dir.x > 0.3f && dir.y < -0.3f)
            {
                sr.sprite = downRight[0];
            }
            else if (dir.x < -0.3f && dir.y < -0.3f)
            {
                sr.sprite = downLeft[0];
            }
            else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    sr.sprite = right[0];
                }
                else
                {
                    sr.sprite = left[0];
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    sr.sprite = up[0];
                }
                else
                {
                    sr.sprite = down[0];
                }
            }
        }
    }

}
