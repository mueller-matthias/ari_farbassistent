using UnityEngine;

[RequireComponent(typeof(UnityEngine.XR.ARFoundation.ARPlaneManager))]
public class WallPainterController : MonoBehaviour
{
    [SerializeField] private Material wallMaterial;

    private UnityEngine.XR.ARFoundation.ARPlaneManager planeManager;

    private readonly System.Collections.Generic.Dictionary<
        UnityEngine.XR.ARSubsystems.TrackableId,
        UnityEngine.MeshRenderer
    > rendererCache = new();

    void Awake()
    {
        planeManager = GetComponent<UnityEngine.XR.ARFoundation.ARPlaneManager>();

        planeManager.requestedDetectionMode =
            UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Vertical;

        if (wallMaterial == null)
            Debug.LogWarning("WallPainterController: Wall-Material fehlt!");
    }

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
        PaintExistingPlanes();
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
        rendererCache.Clear();
    }

    void OnPlanesChanged(UnityEngine.XR.ARFoundation.ARPlanesChangedEventArgs args)
    {
        PaintPlanes(args.added);
        PaintPlanes(args.updated);

        if (args.removed != null)
        {
            foreach (var plane in args.removed)
                rendererCache.Remove(plane.trackableId);
        }
    }

    public void SetWallColor(Color color)
    {
        if (wallMaterial == null) return;
        wallMaterial.color = color;
        PaintExistingPlanes();
    }

    private void PaintExistingPlanes()
    {
        foreach (var plane in planeManager.trackables)
            PaintPlane(plane);
    }

    private void PaintPlanes(System.Collections.Generic.IEnumerable<UnityEngine.XR.ARFoundation.ARPlane> planes)
    {
        if (planes == null) return;
        foreach (var plane in planes)
            PaintPlane(plane);
    }

    private bool IsWall(UnityEngine.XR.ARFoundation.ARPlane plane)
    {
        if (plane == null) return false;

        if (plane.alignment != UnityEngine.XR.ARSubsystems.PlaneAlignment.Vertical)
            return false;

        if (plane.size.x * plane.size.y < 0.02f)
            return false;

        return true;
    }

    private void PaintPlane(UnityEngine.XR.ARFoundation.ARPlane plane)
    {
        if (wallMaterial == null || plane == null) return;
        if (!IsWall(plane)) return;

        if (!rendererCache.TryGetValue(plane.trackableId, out var renderer) || renderer == null)
        {
            renderer = plane.GetComponentInChildren<UnityEngine.MeshRenderer>();
            if (renderer == null)
            {
                Debug.LogWarning("WallPainterController: Plane hat keinen MeshRenderer.");
                return;
            }
            rendererCache[plane.trackableId] = renderer;
        }

        renderer.material = wallMaterial;
    }
}
