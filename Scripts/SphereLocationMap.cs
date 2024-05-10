using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereLocationMap : MonoBehaviour
{
    [SerializeField] private Transform model3D;

    [SerializeField] private Transform minimap;

    private void Update() {
        Vector3 localPosition = (minimap.transform.position - model3D.position) * model3D.localScale.x / minimap.localScale.x / 3;
        gameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.z, localPosition.y);
    }
}
