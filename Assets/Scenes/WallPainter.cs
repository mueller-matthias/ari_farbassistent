using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARPlaneManager))]
public class WallPainter : MonoBehaviour
{
    [SerializeField] private Material wallMaterial;

    private ARPlaneManager planeManager;
    private readonly Dictionary<TrackableId, MeshRenderer> rendererCache = new();

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();

        // Nur vertikale Planes erkennen
        planeManager.requestedDetectionMode = PlaneDetectionMode.Vertical;

        if (wallMaterial == null)
            Debug.LogWarning("WallPainter: Kein Wall-Material zugewiesen!");
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

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        PaintPlanes(args.added);
        PaintPlanes(args.updated);

        if (args.removed != null)
        {
            foreach (var plane in args.removed)
                rendererCache.Remove(plane.trackableId);
        }
    }

    private void PaintExistingPlanes()
    {
        foreach (var plane in planeManager.trackables)
        {
            PaintPlane(plane);
        }
    }

    private void PaintPlanes(IEnumerable<ARPlane> planes)
    {
        if (planes == null) return;

        foreach (var plane in planes)
            PaintPlane(plane);
    }

    private bool IsWall(ARPlane plane)
    {
        if (plane == null)
            return false;

        // Alignment muss vertikal sein
        if (plane.alignment != PlaneAlignment.Vertical)
            return false;

        // Wenn Classification verfügbar ist (v.a. iOS/ARKit), zusätzlich auf "Wall" prüfen
        // Auf Plattformen ohne Classification ist das meist PlaneClassification.None
        return plane.classification == PlaneClassification.Wall;
    }

    private void PaintPlane(ARPlane plane)
    {
        if (wallMaterial == null || plane == null)
            return;

        // Nur Wände
        if (!IsWall(plane))
        {
            // Debug.Log($"Plane ist keine Wand: {plane.trackableId}, Alignment: {plane.alignment}, Class: {plane.classification}");
            return;
        }

        if (!rendererCache.TryGetValue(plane.trackableId, out var renderer) || renderer == null)
        {
            renderer = plane.GetComponentInChildren<MeshRenderer>();
            if (renderer == null)
            {
                Debug.LogWarning($"WallPainter: Kein MeshRenderer für Plane {plane.trackableId} gefunden.");
                return;
            }

            rendererCache[plane.trackableId] = renderer;
        }

        renderer.material = wallMaterial;
        // Debug.Log($"Wand eingefärbt: {plane.trackableId}");
    }
}
