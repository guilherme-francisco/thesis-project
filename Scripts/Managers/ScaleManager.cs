using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleManager : MonoBehaviour
{   
    public static ScaleManager Instance { get; private set; }

    [SerializeField] private float originalScale = 1/1000f;
    [SerializeField] private GameObject model3D;



    private float currentScale;

    private void Awake() {
        Instance = this;
        currentScale = model3D.transform.localScale.y;
    }

    public float GetRealMeasurement(float value) {
        return value / (originalScale / currentScale);
    }    
}
