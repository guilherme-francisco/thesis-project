using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeasureByPosition : MonoBehaviour
{
    public static MeasureByPosition Instance { get; private set; }
    public event EventHandler<OnMeasurementEventArgs> OnMeasurementEvent;
    public class OnMeasurementEventArgs : EventArgs
    {
        public float measurementValue;
    }

    [SerializeField] private GameObject leftControllers;
    [SerializeField] private GameObject rightControllers;
    [SerializeField] private GameObject leftHandSphere;
    [SerializeField] private GameObject rightHandSphere;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        InputActionsManager.Instance.InputActions.XRILeftHandInteraction.Select.performed += LeftHand_OnSelectPerformed;
        InputActionsManager.Instance.InputActions.XRIRightHandInteraction.Select.performed += RightHand_OnSelectPerformed;

        Hide();
    }

    private void LeftHand_OnSelectPerformed(InputAction.CallbackContext context)
    {

        if (ToolsPanelUI.Instance.GetMode() != ToolsPanelUI.Modes.Measure ||
        (MeasurementToolsUI.Instance.GetMeasurementTypes() != MeasurementToolsUI.MeasurementTypes.Linear  &&
            MeasurementManager.Instance.GetCurrentCurvedMeasurementMethod() != MeasurementManager.CurvedMeasurementMethods.HandPosition)) {
            return;
        }

        leftHandSphere.SetActive(true);
        float sphereRadius = GameManager.Instance.GetSphereRadiusSize();
        
        leftHandSphere.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);

        leftHandSphere.transform.position = leftControllers.transform.position;
        MakeMeasurement();
    }


    private void RightHand_OnSelectPerformed(InputAction.CallbackContext context)
    {
         if (ToolsPanelUI.Instance.GetMode() != ToolsPanelUI.Modes.Measure ||
            (MeasurementToolsUI.Instance.GetMeasurementTypes() != MeasurementToolsUI.MeasurementTypes.Linear  &&
            MeasurementManager.Instance.GetCurrentCurvedMeasurementMethod() != MeasurementManager.CurvedMeasurementMethods.HandPosition)) {
            return;
        }

        rightHandSphere.SetActive(true);
        float sphereRadius = GameManager.Instance.GetSphereRadiusSize();
        
        rightHandSphere.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);

        rightHandSphere.transform.position = rightControllers.transform.position;
        MakeMeasurement();
    }

    private void MakeMeasurement() {
        if (leftHandSphere.activeSelf && rightHandSphere.activeSelf) {
            float distance = Vector3.Distance(leftHandSphere.transform.position, rightHandSphere.transform.position);
            Debug.Log("Distance between hands: " + distance);

            OnMeasurementEvent?.Invoke(this, new OnMeasurementEventArgs { measurementValue = distance });
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Update() {
        if (MenuToolsUI.Instance.isActiveAndEnabled || ToolsPanelUI.Instance.isActiveAndEnabled)
            Hide();
    }

}
