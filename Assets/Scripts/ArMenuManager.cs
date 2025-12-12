using UnityEngine;

public class ARMenuManager : MonoBehaviour
{
    [SerializeField] private WallPainter wallPainter;

    public void PickHex(string hex)
    {
        if (wallPainter == null) return;
        if (ColorUtility.TryParseHtmlString(hex, out var c))
            wallPainter.SetWallColor(c);
    }
}
