using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARPlaneManager))]
public class WallPainter : MonoBehaviour
{
    [SerializeField] private Material wallMaterial;

    private ARPlaneManager planeManager;

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        // Neue und aktualisierte Planes behandeln
        PaintPlanes(args.added);
        PaintPlanes(args.updated);
    }

    private void PaintPlanes(IEnumerable<ARPlane> planes)
    {
        foreach (var plane in planes)
        {
            // Nur vertikale Flächen / Wände
            if(plane.alignment == PlaneAlignment.Vertical)


            {
                var renderer = plane.GetComponent<MeshRenderer>();
                if (renderer != null && wallMaterial != null)
                {
                    renderer.material = wallMaterial;
                }
            }
        }
    }

}
