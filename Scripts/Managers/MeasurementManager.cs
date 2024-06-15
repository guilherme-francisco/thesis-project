using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeasurementManager : MonoBehaviour
{   
    public enum LinearMeasurementMethods {
        Sphere,
        TwoHands,
        HandPosition
    }

    public static MeasurementManager Instance { get; private set; }
    public event EventHandler<OnMeasurementEventArgs> OnMeasurementEvent;
    public class OnMeasurementEventArgs : EventArgs
    {
        public float measurementValue;
    }

    [Header("Linear")]
    [SerializeField] private GameObject measureByPosition;
    [SerializeField] private GameObject measureByHands;
    [SerializeField] private float sphereRadius = 0.1f;
    [SerializeField] private GameObject spherePrefab;

    private MeasurementToolsUI measurementToolsUI;
    private readonly List<GameObject> spheres = new();

    [Header("Curved")]
    [SerializeField] private GameObject curvedLineRenderer;
    [SerializeField] private GameObject CurvedLinePoint;

    [Header("Radius")]
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Scale")]
    [SerializeField] private Transform xrOrigin;

    // MeasurementMethods
    private LinearMeasurementMethods currentLinearMeasurementMethod = LinearMeasurementMethods.Sphere;

    private void Awake() {
        Instance = this;
    }

    void Start () {
        measurementToolsUI = MeasurementToolsUI.Instance;

		InputActionsManager.Instance.InputActions.XRILeftHand.Select.performed += OnSelectPerformed;
        CurvedLineRenderer.Instance.OnMeasurementEvent += CurvedLineRenderer_OnMeasurementEvent;
        MeasureByPosition.Instance.OnMeasurementEvent += MeasureByPosition_OnMeasurementEvent;
        measurementToolsUI.OnMeasurementTypeChange += MeasurementToolsUI_OnMeasurementTypeChange;
    }

    private void MeasureByPosition_OnMeasurementEvent(object sender, MeasureByPosition.OnMeasurementEventArgs e)
    {
        OnMeasurementEvent?.Invoke(this, new OnMeasurementEventArgs {
            measurementValue = e.measurementValue,
        });
    }


    private void MeasurementToolsUI_OnMeasurementTypeChange(object sender, EventArgs e)
    {
        if (measurementToolsUI.GetMeasurementTypes() == MeasurementToolsUI.MeasurementTypes.Radius) {
            HandleRadiusMeasure();
        } else if (measurementToolsUI.GetMeasurementTypes() == MeasurementToolsUI.MeasurementTypes.Linear) {
            if (currentLinearMeasurementMethod == LinearMeasurementMethods.TwoHands) {
                measureByHands.SetActive(true);
            } else if (currentLinearMeasurementMethod == LinearMeasurementMethods.HandPosition) {
                measureByPosition.SetActive(true);
            }
        } else {
            circlePrefab.SetActive(false);
        }
    }


    private void CurvedLineRenderer_OnMeasurementEvent(object sender, CurvedLineRenderer.OnMeasurementEventArgs e)
    {
        OnMeasurementEvent?.Invoke(this, new OnMeasurementEventArgs {
            measurementValue = e.measurementValue,
        });
    }


    private void OnSelectPerformed(InputAction.CallbackContext context)
    {
        if (ToolsPanelUI.Instance.GetMode() == ToolsPanelUI.Modes.Measure) {
            switch (measurementToolsUI.GetMeasurementTypes())
            {
                case MeasurementToolsUI.MeasurementTypes.Linear:
                    if(currentLinearMeasurementMethod == LinearMeasurementMethods.Sphere) {
                        HandleLinearMeasure();
                    }
                    break;
                case MeasurementToolsUI.MeasurementTypes.Curved:
                    HandleCurvedMeasure();
                    break;
            }
        }
    }

    private void HandleRadiusMeasure()
    {
        if (!circlePrefab.activeSelf)
        {           
            circlePrefab.SetActive(true);
            circlePrefab.transform.position = spawnPoint.position;
            if (ToolsPanelUI.Instance.GetNavigation() == ToolsPanelUI.Navigation.Inside) {
                circlePrefab.transform.localScale =  xrOrigin.localScale;
            }
        }
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

    public LinearMeasurementMethods GetCurrentLinearMeasurementMethod() {
        return currentLinearMeasurementMethod;
    }

    public void SetCurrentLinearMeasurementMethod(LinearMeasurementMethods linearMeasurementMethod) {
        currentLinearMeasurementMethod = linearMeasurementMethod;
    }

}
