using UnityEngine;

public class ARMenuManager : MonoBehaviour
{
    [SerializeField] private WallPainterController wallPainter;

    public void PickHex(string hex)
    {
        Debug.Log("PickHex called: " + hex, this);

        if (wallPainter == null)
        {
            Debug.LogError("ARMenuManager: wallPainter ist nicht gesetzt!", this);
            return;
        }

        if (ColorUtility.TryParseHtmlString(hex, out Color c))
        {
            wallPainter.SetWallColor(c);
        }
        else
        {
            Debug.LogError("Ungültiger Hex-Wert: " + hex, this);
        }
    }
}