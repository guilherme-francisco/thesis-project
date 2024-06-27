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

    public enum CurvedMeasurementMethods {
        
        Sphere,
        HandPosition
    }

    public enum RadiusMeasurementMethods {
        Sphere,
        Prefab
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

    [SerializeField] private GameObject  curvedMeasureByPosition;

    [Header("Radius")]
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject circleByThreePoints;
    [Header("Scale")]
    [SerializeField] private Transform xrOrigin;

    // MeasurementMethods
    private LinearMeasurementMethods currentLinearMeasurementMethod = LinearMeasurementMethods.Sphere;

    private CurvedMeasurementMethods currentCurvedMeasurementMethod = CurvedMeasurementMethods.Sphere;

    private RadiusMeasurementMethods currentRadiusMeasurementMethod = RadiusMeasurementMethods.Prefab;

    private void Awake() {
        Instance = this;
    }

    void Start () {
        measurementToolsUI = MeasurementToolsUI.Instance;

		InputActionsManager.Instance.InputActions.XRILeftHand.Select.performed += OnSelectPerformed;
        curvedLineRenderer.GetComponent<CurvedLineRenderer>().OnMeasurementEvent += CurvedLineRenderer_OnMeasurementEvent;
        measureByHands.GetComponent<CurvedLineRenderer>().OnMeasurementEvent += CurvedLineRenderer_OnMeasurementEvent;
        MeasureByPosition.Instance.OnMeasurementEvent += MeasureByPosition_OnMeasurementEvent;
        measurementToolsUI.OnMeasurementTypeChange += MeasurementToolsUI_OnMeasurementTypeChange;
    }

    private void MeasureByPosition_OnMeasurementEvent(object sender, MeasureByPosition.OnMeasurementEventArgs e)
    {
        OnMeasurementEvent?.Invoke(this, new OnMeasurementEventArgs {
            measurementValue = e.measurementValue,
        });
    }

    private void DisableUIs() {
        circlePrefab.SetActive(false);
        circleByThreePoints.SetActive(false);
        measureByHands.SetActive(false);
        measureByPosition.SetActive(false);
        curvedMeasureByPosition.SetActive(false);
        circleByThreePoints.SetActive(false);
    }

    private void MeasurementToolsUI_OnMeasurementTypeChange(object sender, EventArgs e)
    {
        DisableUIs();

        if (measurementToolsUI.GetMeasurementTypes() == MeasurementToolsUI.MeasurementTypes.Radius) {
            HandleRadiusMeasure();
        } else if (measurementToolsUI.GetMeasurementTypes() == MeasurementToolsUI.MeasurementTypes.Linear) {
            if (currentLinearMeasurementMethod == LinearMeasurementMethods.TwoHands) {
                measureByHands.SetActive(true);
            } else if (currentLinearMeasurementMethod == LinearMeasurementMethods.HandPosition) {
                measureByPosition.SetActive(true);
            }
        } else if (measurementToolsUI.GetMeasurementTypes() == MeasurementToolsUI.MeasurementTypes.Curved) {
            if (currentCurvedMeasurementMethod == CurvedMeasurementMethods.HandPosition) {
                curvedMeasureByPosition.SetActive(true);
            }
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
                    if (currentCurvedMeasurementMethod == CurvedMeasurementMethods.Sphere) {
                        HandleCurvedMeasure();
                    } else if (currentCurvedMeasurementMethod == CurvedMeasurementMethods.HandPosition) {
                        curvedMeasureByPosition.SetActive(true);
                    }
                    break;
                case MeasurementToolsUI.MeasurementTypes.Radius:
                    HandleRadiusMeasure();
                    break;
            }
        }
    }

    private void HandleRadiusMeasure()
    {
        
        Debug.Log(InputActionsManager.Instance.InputActions.XRILeftHand.Select.phase);
        if (!circlePrefab.activeSelf && currentRadiusMeasurementMethod == RadiusMeasurementMethods.Prefab)
        {           
            circlePrefab.SetActive(true);
            circlePrefab.transform.position = spawnPoint.position;
            if (ToolsPanelUI.Instance.GetNavigation() == ToolsPanelUI.Navigation.Inside) {
                circlePrefab.transform.localScale =  xrOrigin.localScale;
            }
        }
        else if (currentRadiusMeasurementMethod == RadiusMeasurementMethods.Sphere && InputActionsManager.Instance.InputActions.XRILeftHand.Select.triggered) {
            if (spheres.Count >= 3)
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

            if (spheres.Count == 3)
            {
                
                
                circleByThreePoints.SetActive(true);
                
                Vector3 center = CircleByThreePoints.Instance.CreateCircle(spheres[0].transform.position, 
                    spheres[1].transform.position, 
                    spheres[2].transform.position
                );
                
                float radius = Vector3.Distance(spheres[0].transform.position, spheres[1].transform.position);

                
                Debug.Log("Distance between spheres: " + radius);
                
                OnMeasurementEvent?.Invoke(this, new OnMeasurementEventArgs
                {
                    measurementValue = radius
                });
            }
        }
    }


    private void HandleCurvedMeasure()
    {
        GameManager.Instance.TryToCreatePrefab(CurvedLinePoint, curvedLineRenderer);
    }

    public void HandleCurvedMeasureByHandPosition(Vector3 position, Vector3 scale) {
        GameObject prefab = Instantiate(CurvedLinePoint, position, Quaternion.identity, curvedLineRenderer.transform);

        prefab.transform.localScale = scale;
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

    public CurvedMeasurementMethods GetCurrentCurvedMeasurementMethod() {
        return currentCurvedMeasurementMethod;
    }

    public void SetCurrentCurvedMeasurementMethod(CurvedMeasurementMethods curvedMeasurementMethod) {
        currentCurvedMeasurementMethod = curvedMeasurementMethod;
    }

    public RadiusMeasurementMethods GetCurrentRadiusMeasurementMethod() {
        return currentRadiusMeasurementMethod;
    }

    public void SetCurrentRadiusMeasurementMethod(RadiusMeasurementMethods radiusMeasurementMethod) {
        currentRadiusMeasurementMethod = radiusMeasurementMethod;
    }

}
