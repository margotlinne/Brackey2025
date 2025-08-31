using UnityEngine;
using UnityEngine.EventSystems;

namespace Margot
{
    public class HoverImage : SoundPlayer, IPointerEnterHandler, IPointerExitHandler
    {
        public Transform block;  
        private Vector3 originalPos;
        private bool isHovered = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySound(0, 0.2f);
            if (block == null || isHovered) return;

            isHovered = true;
            originalPos = block.localPosition;

            block.localPosition = new Vector3(
                originalPos.x,
                originalPos.y + 50f,
                originalPos.z
            );
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlaySound(1, 0.2f);
            if (block == null || !isHovered) return;

            isHovered = false;
            block.localPosition = originalPos;
        }
    }
}
