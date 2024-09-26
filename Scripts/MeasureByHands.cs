using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class MeasureByHands : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ToolsPanelUI.Instance.OnModeChange += ToolsPanelUI_OnModeChange;
        InputActionsManager.Instance.InputActions.XRILeftHand.MenuButton.performed += Righthand_OnMenuButtonPerformed;
        Hide();
    }

    private void Righthand_OnMenuButtonPerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction && ToolsPanelUI.Instance.isActiveAndEnabled) {
            if(ToolsPanelUI.Instance.GetMode() == ToolsPanelUI.Modes.Measure && 
                MeasurementToolsUI.Instance.GetMeasurementTypes() == MeasurementToolsUI.MeasurementTypes.Linear &&
                MeasurementManager.Instance.GetCurrentLinearMeasurementMethod() == MeasurementManager.LinearMeasurementMethods.TwoHands)
                gameObject.SetActive(true);
        }
    }


    private void ToolsPanelUI_OnModeChange(object sender, EventArgs e)
    {
        if (ToolsPanelUI.Instance.GetMode() != ToolsPanelUI.Modes.Measure || 
            MeasurementToolsUI.Instance.GetMeasurementTypes() != MeasurementToolsUI.MeasurementTypes.Linear) {
                gameObject.SetActive(false);
            }
    }


    private void Update() {
        if (MenuToolsUI.Instance.isActiveAndEnabled || ToolsPanelUI.Instance.isActiveAndEnabled)
            Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
