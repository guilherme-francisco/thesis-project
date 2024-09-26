using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CurvedMeasureByPosition : MonoBehaviour
{
    public static CurvedMeasureByPosition Instance { get; private set; }

    [SerializeField] private GameObject leftControllers;
    [SerializeField] private GameObject rightControllers;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        InputActionsManager.Instance.InputActions.XRILeftHand.Select.performed += LeftHand_OnSelectPerformed;
        InputActionsManager.Instance.InputActions.XRIRightHand.Select.performed += RightHand_OnSelectPerformed;

        Hide();
    }

    private void LeftHand_OnSelectPerformed(InputAction.CallbackContext context)
    {
        if (ToolsPanelUI.Instance.GetMode() != ToolsPanelUI.Modes.Measure ||
            MeasurementToolsUI.Instance.GetMeasurementTypes() != MeasurementToolsUI.MeasurementTypes.Curved  ||
            MeasurementManager.Instance.GetCurrentCurvedMeasurementMethod() != MeasurementManager.CurvedMeasurementMethods.HandPosition) {
            return;
        }

        float sphereRadius = GameManager.Instance.GetSphereRadiusSize();
        
        Vector3 localScale = new(sphereRadius, sphereRadius, sphereRadius);

        MeasurementManager.Instance.HandleCurvedMeasureByHandPosition(leftControllers.transform.position, localScale);
    }


    private void RightHand_OnSelectPerformed(InputAction.CallbackContext context)
    {
        if (ToolsPanelUI.Instance.GetMode() != ToolsPanelUI.Modes.Measure ||
            MeasurementToolsUI.Instance.GetMeasurementTypes() != MeasurementToolsUI.MeasurementTypes.Curved  ||
            MeasurementManager.Instance.GetCurrentCurvedMeasurementMethod() != MeasurementManager.CurvedMeasurementMethods.HandPosition) {
            return;
        }
        
        float sphereRadius = GameManager.Instance.GetSphereRadiusSize();
        
        Vector3 localScale = new(sphereRadius, sphereRadius, sphereRadius);

        MeasurementManager.Instance.HandleCurvedMeasureByHandPosition(rightControllers.transform.position, localScale);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Update() {
        if (MenuToolsUI.Instance.isActiveAndEnabled || ToolsPanelUI.Instance.isActiveAndEnabled)
            Hide();
    }

}
