using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeasurementManager : MonoBehaviour
{
    public static MeasurementManager Instance { get; private set; }
    public event EventHandler<OnMeasurementEventArgs> OnMeasurementEvent;
    public class OnMeasurementEventArgs : EventArgs
    {
        public float measurementValue;
    }

    // Linear
    [SerializeField] private float sphereRadius = 0.1f;
    [SerializeField] private GameObject spherePrefab;

    private MeasurementToolsUI measurementToolsUI;
    private List<GameObject> spheres = new();

    // Curved
    [SerializeField] private GameObject curvedLineRenderer;
    [SerializeField] private GameObject CurvedLinePoint;

    // Radius
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private Transform spawnPoint;

    private void Awake() {
        Instance = this;
    }

    void Start () {
        measurementToolsUI = MeasurementToolsUI.Instance;

		InputActionsManager.Instance.InputActions.XRILeftHand.Select.performed += OnSelectPerformed;
    }

    private void OnSelectPerformed(InputAction.CallbackContext context)
    {
        if (ToolsPanelUI.Instance.GetMode() == ToolsPanelUI.Modes.Measure) {
            switch (measurementToolsUI.GetMeasurementTypes())
            {
                case MeasurementToolsUI.MeasurementTypes.Linear:
                    HandleLinearMeasure();
                    break;
                case MeasurementToolsUI.MeasurementTypes.Curved:
                    HandleCurvedMeasure();
                    break;
                case MeasurementToolsUI.MeasurementTypes.Radius:
                    HandleRadiusMeasure();
                    break;
                default:
                    circlePrefab.SetActive(false);
                    break;
            }
        }
    }

    private void HandleRadiusMeasure()
    {
        circlePrefab.SetActive(true);
        circlePrefab.transform.position = spawnPoint.position;
    }


    private void HandleCurvedMeasure()
    {
        GameManager.Instance.TryToCreatePrefab(CurvedLinePoint, curvedLineRenderer);
    }


    private void HandleLinearMeasure()
    {
        if (spheres.Count >= 2)
        {
            foreach (GameObject sphere in spheres)
            {
                Destroy(sphere);
            }
            spheres.Clear();
        }

        if (GameManager.Instance.TryToCreateSphere(out GameObject newSphere))
        {
            spheres.Add(newSphere);
        }

        if (spheres.Count == 2)
        {
            float distance = Vector3.Distance(spheres[0].transform.position, spheres[1].transform.position);
            Debug.Log("Distance between spheres: " + distance);

            OnMeasurementEvent?.Invoke(this, new OnMeasurementEventArgs
            {
                measurementValue = distance
            });
        }
    } 


}
