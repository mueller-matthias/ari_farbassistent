using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform rt;
    Rect last;

    void Awake() => rt = GetComponent<RectTransform>();

    void Update()
    {
        if (Screen.safeArea == last) return;
        last = Screen.safeArea;

        var min = last.position;
        var max = last.position + last.size;

        min.x /= Screen.width; min.y /= Screen.height;
        max.x /= Screen.width; max.y /= Screen.height;

        rt.anchorMin = min;
        rt.anchorMax = max;
    }
}
