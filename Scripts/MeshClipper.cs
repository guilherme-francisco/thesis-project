using UnityEngine;
using System.Collections.Generic;

public class MeshClipper : MonoBehaviour
{
    public GameObject model3D;
    public Transform planeTransform;
    private Material cuttingMaterial;

    private void Awake() {
        MeshRenderer renderer = model3D.GetComponent<MeshRenderer>();
        cuttingMaterial = renderer.material;
    }

    void Update()
    {
        Vector3 planePosition = planeTransform.position;
        Vector3 planeNormal = planeTransform.up * -1;

        cuttingMaterial.SetVector("_PlanePosition", planePosition);
        cuttingMaterial.SetVector("_PlaneNormal", planeNormal);
    }
}
