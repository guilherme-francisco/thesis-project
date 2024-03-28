using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System;

public class SphereCreator : MonoBehaviour
{   
    public static SphereCreator Instance { get; private set; }

    [SerializeField] private float sphereRadius = 1.0f;
    [SerializeField] private GameObject spherePrefab;

    public event EventHandler<OnMeasurementEventArgs> OnMeasurementEvent;
    public class OnMeasurementEventArgs : EventArgs {
        public float measurementValue;
    }

    private XRBaseInteractor rayInteractor;
    private XRController xrController;

    private List<GameObject> spheres = new List<GameObject>();

     private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate instance of ScriptWithEvent. Destroying the duplicate.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        rayInteractor = GetComponent<XRBaseInteractor>();

        xrController = GetComponent<XRController>();
        if (rayInteractor != null)
        {
            rayInteractor.selectEntered.AddListener(CreateSphere);
        }
        else
        {
            Debug.LogWarning("No XRBaseInteractor component found on the GameObject.", this);
        }
    }

    private void OnDestroy()
    {
        if (rayInteractor != null)
        {
            rayInteractor.selectEntered.RemoveListener(CreateSphere);
        }
    }

    private void CreateSphere(SelectEnterEventArgs args)
    {
        if (spheres.Count >= 2)
        {
            RemoveAllSpheres();
        }

        RaycastHit hit;
        if (Physics.Raycast(rayInteractor.transform.position, rayInteractor.transform.forward, out hit))
        {
            GameObject sphere = Instantiate(spherePrefab, hit.point, Quaternion.identity);
            sphere.transform.localScale = Vector3.one * sphereRadius * 2f;

            spheres.Add(sphere);
        }

        if (spheres.Count == 2)
        {
            MeasureDistanceBetweenSpheres();
            return;
        }
    }

    private void MeasureDistanceBetweenSpheres()
    {
        float distance = Vector3.Distance(spheres[0].transform.position, spheres[1].transform.position);
        Debug.Log("Distance between spheres: " + distance);

        OnMeasurementEvent?.Invoke(this, new OnMeasurementEventArgs {
            measurementValue = distance
        });
    }

    private void RemoveAllSpheres()
    {
        foreach (GameObject sphere in spheres)
        {
            Destroy(sphere);
        }
        spheres.Clear();
    }
}
