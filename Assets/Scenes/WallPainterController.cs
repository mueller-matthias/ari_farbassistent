using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class WallPainterController : MonoBehaviour
{
    [SerializeField] private Material wallMaterial;
    [SerializeField] private float minWallArea = 0.02f;

    private ARPlaneManager planeManager;
<<<<<<< HEAD
<<<<<<< HEAD
    private Material runtimeWallMaterial;
=======
>>>>>>> 5cb32c9 (correction)

=======
>>>>>>> 64a478e (AR Projektion verbessert)
    private readonly Dictionary<TrackableId, MeshRenderer> rendererCache = new();

    private void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
<<<<<<< HEAD
<<<<<<< HEAD
        planeManager.requestedDetectionMode = PlaneDetectionMode.Vertical;

        if (wallMaterial == null)
        {
            Debug.LogError("WallPainterController: wallMaterial fehlt!", this);
            return;
        }

        // Eigene Runtime-Kopie erzeugen
        runtimeWallMaterial = new Material(wallMaterial);
=======

        if (planeManager != null)
        {
            planeManager.requestedDetectionMode = PlaneDetectionMode.Vertical;
        }
=======
        planeManager.requestedDetectionMode = PlaneDetectionMode.Vertical;
>>>>>>> 64a478e (AR Projektion verbessert)

        if (wallMaterial == null)
        {
            Debug.LogWarning("WallPainterController: Wall-Material fehlt!");
        }
>>>>>>> 5cb32c9 (correction)
    }

    private void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
        PaintExistingPlanes();
    }

    private void OnDisable()
    {
<<<<<<< HEAD
        if (planeManager != null)
<<<<<<< HEAD
            planeManager.planesChanged -= OnPlanesChanged;
=======
        {
            planeManager.planesChanged -= OnPlanesChanged;
        }
>>>>>>> 5cb32c9 (correction)

=======
        planeManager.planesChanged -= OnPlanesChanged;
>>>>>>> 64a478e (AR Projektion verbessert)
        rendererCache.Clear();
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        PaintPlanes(args.added);
        PaintPlanes(args.updated);

        foreach (var plane in args.removed)
        {
            rendererCache.Remove(plane.trackableId);
        }
    }

    public void SetWallColor(Color color)
    {
<<<<<<< HEAD
<<<<<<< HEAD
        if (runtimeWallMaterial == null)
        {
            Debug.LogError("SetWallColor: runtimeWallMaterial ist NULL!", this);
            return;
        }

        // Für Standard-Shader
        if (runtimeWallMaterial.HasProperty("_Color"))
            runtimeWallMaterial.SetColor("_Color", color);

        // Für URP/HDRP/Lit
        if (runtimeWallMaterial.HasProperty("_BaseColor"))
            runtimeWallMaterial.SetColor("_BaseColor", color);

        Debug.Log("Neue Wandfarbe gesetzt: " + color, this);

        // sicherheitshalber allen vorhandenen Planes erneut zuweisen
        PaintExistingPlanes();
=======
        if (wallMaterial == null) return;

        wallMaterial.color = color;
>>>>>>> 5cb32c9 (correction)
=======
        if (wallMaterial != null)
        {
            wallMaterial.color = color;
        }
>>>>>>> 64a478e (AR Projektion verbessert)
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
        foreach (var plane in planes)
        {
            PaintPlane(plane);
        }
    }

    private bool IsWall(ARPlane plane)
    {
<<<<<<< HEAD
        if (plane == null) return false;
<<<<<<< HEAD
        if (plane.alignment != PlaneAlignment.Vertical) return false;
        if (plane.size.x * plane.size.y < 0.02f) return false;
=======

        if (plane.alignment != PlaneAlignment.Vertical)
            return false;

        if (plane.size.x * plane.size.y < minWallArea)
            return false;
>>>>>>> 5cb32c9 (correction)

        return true;
=======
        return plane != null &&
               plane.alignment == PlaneAlignment.Vertical &&
               plane.size.x * plane.size.y >= minWallArea;
>>>>>>> 64a478e (AR Projektion verbessert)
    }

    private void PaintPlane(ARPlane plane)
    {
<<<<<<< HEAD
        if (runtimeWallMaterial == null || plane == null) return;
        if (!IsWall(plane)) return;

        if (!rendererCache.TryGetValue(plane.trackableId, out var renderer) || renderer == null)
        {
            renderer = plane.GetComponentInChildren<MeshRenderer>();

            if (renderer == null)
            {
                Debug.LogWarning("WallPainterController: Plane hat keinen MeshRenderer.", plane);
                return;
            }

            rendererCache[plane.trackableId] = renderer;
        }
=======
        if (wallMaterial == null || !IsWall(plane))
            return;

        if (!TryGetRenderer(plane, out var renderer))
            return;
>>>>>>> 64a478e (AR Projektion verbessert)

<<<<<<< HEAD
        // sharedMaterial ist hier besser, weil alle dieselbe Runtime-Kopie nutzen sollen
        renderer.sharedMaterial = runtimeWallMaterial;
=======
        if (renderer.sharedMaterial != wallMaterial)
        {
            renderer.sharedMaterial = wallMaterial;
        }
>>>>>>> 5cb32c9 (correction)
    }

    private bool TryGetRenderer(ARPlane plane, out MeshRenderer renderer)
    {
        if (rendererCache.TryGetValue(plane.trackableId, out renderer) && renderer != null)
        {
            return true;
        }

        renderer = plane.GetComponentInChildren<MeshRenderer>();

        if (renderer == null)
        {
            Debug.LogWarning("WallPainterController: Plane hat keinen MeshRenderer.");
            return false;
        }

        rendererCache[plane.trackableId] = renderer;
        return true;
    }
}