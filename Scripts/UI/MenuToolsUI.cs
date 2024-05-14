using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuToolsUI : MonoBehaviour
{
    [SerializeField] private Button leaveButton;
    [SerializeField] private Button dicomButton;
    [SerializeField] private Button measurementButton;

    [SerializeField] private Button mapDisplayButton;

    [SerializeField] private GameObject dicomImageUI;
    [SerializeField] private GameObject sliders;

    [SerializeField] private GameObject minimap;
    [SerializeField] private GameObject measurementToolsUI;    
    [SerializeField] private Transform xrOrigin;

    private ToolsPanelUI toolsPanelUI;

    private void Start() {
        toolsPanelUI = ToolsPanelUI.Instance;
        
        leaveButton.onClick.AddListener(OnLeaveClick);
        dicomButton.onClick.AddListener(OnDicomClick);
        measurementButton.onClick.AddListener(OnMeasurementClick);
        mapDisplayButton.onClick.AddListener(OnMapDisplayClick);

        XRIDefaultInputActions inputActions = InputActionsManager.Instance.InputActions;

        inputActions.XRIRightHand.MenuButton.performed += OnMenuButtonPerformed;

        minimap.SetActive(false);
        Hide();
    }

    private void OnMenuButtonPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Right Menu Button performed!");

        if (minimap.activeSelf || dicomImageUI.activeSelf || measurementToolsUI.activeSelf) 
        {
            minimap.SetActive(false);
            dicomImageUI.SetActive(false);
            measurementToolsUI.SetActive(false);
            return;
        } else if (toolsPanelUI.GetNavigation() == ToolsPanelUI.Navigation.Inside) {
            Debug.Log("Menu mode:" + gameObject.activeSelf.ToString());
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    private void OnMapDisplayClick()
    {
        minimap.SetActive(true);
        Hide();
    }


    private void Hide(){
        gameObject.SetActive(false);
    }

    private void OnMeasurementClick()
    {
        measurementToolsUI.SetActive(true);
        toolsPanelUI.SetMode(ToolsPanelUI.Modes.Measure);
        Hide();
    }


    private void OnDicomClick()
    {
        dicomImageUI.SetActive(true);
        sliders.SetActive(false);
        Hide();
    }


    private void OnLeaveClick()
    {
        xrOrigin.localScale *= 10;
        xrOrigin.position = Vector3.zero;
        toolsPanelUI.SetMode(ToolsPanelUI.Modes.Default);
        toolsPanelUI.SetNavigation(ToolsPanelUI.Navigation.Outside);
        Hide();
    }

}
