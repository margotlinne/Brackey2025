using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class AspectRatioEnforcer : MonoBehaviour
{
    public float targetAspect = 16f / 9f;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        ApplyAspect();
    }

    void ApplyAspect()
    {
        float windowAspect = (float)Screen.width / Screen.height;

        if (windowAspect > targetAspect)
        {
            // 모니터 가로가 더 긴 경우 → 좌우 여백(pillarbox)
            float scale = targetAspect / windowAspect;
            Rect rect = new Rect();
            rect.width = scale;
            rect.height = 1f;
            rect.x = (1f - scale) / 2f;
            rect.y = 0;
            cam.rect = rect;
        }
        else if (windowAspect < targetAspect)
        {
            // 모니터 세로가 더 긴 경우 → 상하 여백(letterbox)
            float scale = windowAspect / targetAspect;
            Rect rect = new Rect();
            rect.width = 1f;
            rect.height = scale;
            rect.x = 0;
            rect.y = (1f - scale) / 2f;
            cam.rect = rect;
        }
        else
        {
            // 정확히 16:9면 꽉 채움
            cam.rect = new Rect(0, 0, 1, 1);
        }
    }
}
