using UnityEngine;
using UnityEngine.UI;

namespace Margot
{
    /// <summary>
    /// Custom Image component (Filled type only).
    /// Restricts raycast (hover/click detection) to the actually filled area
    /// instead of the whole rectangular bounds.
    /// </summary>
    public class FilledImageRaycast : Image
    {
        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            // Only works with Filled type
            if (type != Type.Filled)
                return base.IsRaycastLocationValid(screenPoint, eventCamera);

            // Convert screen point to local coordinates
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local);

            // Normalize coordinates relative to pivot
            float x = (local.x / rectTransform.rect.width + rectTransform.pivot.x);
            float y = (local.y / rectTransform.rect.height + rectTransform.pivot.y);

            // Vector from center
            Vector2 dir = new Vector2(x - 0.5f, y - 0.5f);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            // Check if point is inside radius (circle bound)
            float radius = dir.magnitude;
            if (radius > 0.5f) return false;

            // Convert fillAmount to degrees
            float filledAngle = 360f * fillAmount;

            // Determine start angle based on fillOrigin
            // (0 = Right, 1 = Top, 2 = Left, 3 = Bottom)
            float startAngle = 0f;
            switch (fillOrigin)
            {
                case 0: startAngle = 0f; break;   // Right
                case 1: startAngle = 90f; break;  // Top
                case 2: startAngle = 180f; break; // Left
                case 3: startAngle = 270f; break; // Bottom
            }

            // Calculate if angle is inside the filled sector
            bool inside = false;
            if (fillClockwise)
            {
                float endAngle = (startAngle - filledAngle + 360f) % 360f;
                if (startAngle >= endAngle)
                    inside = (angle <= startAngle && angle >= endAngle);
                else
                    inside = (angle <= startAngle || angle >= endAngle);
            }
            else
            {
                float endAngle = (startAngle + filledAngle) % 360f;
                if (endAngle >= startAngle)
                    inside = (angle >= startAngle && angle <= endAngle);
                else
                    inside = (angle >= startAngle || angle <= endAngle);
            }

            return inside;
        }
    }
}
