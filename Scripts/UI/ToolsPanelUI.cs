using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class ToolsPanelUI : MonoBehaviour
{
    public event EventHandler OnNavigateEvent;

    
    [SerializeField] private GameObject dicomImageUI;

    [SerializeField] private GameObject measurementToolsUI; 
    public static ToolsPanelUI Instance { get; private set; }

    public enum Modes
    {
        Default,
        Move,
        Rotate,
        Scale,
        Measure,
        Tag,
        Clip,
        Navigate,
        Dicom
    }

    public enum Navigation {
        Outside,
        Inside,
    }

    [SerializeField] private Button moveButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button scaleButton;
    [SerializeField] private Button measureButton;
    [SerializeField] private Button tagButton;
    [SerializeField] private Button clipButton;
    [SerializeField] private Button navigateButton;
    [SerializeField] private Button dicomButton;

    private Modes currentMode = Modes.Default;

    private Navigation currentNavigation = Navigation.Outside;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Add listeners
        moveButton.onClick.AddListener(() => OnButtonClick(moveButton, Modes.Move));
        rotateButton.onClick.AddListener(() => OnButtonClick(rotateButton, Modes.Rotate));
        scaleButton.onClick.AddListener(() => OnButtonClick(scaleButton, Modes.Scale));
        measureButton.onClick.AddListener(() => OnButtonClick(measureButton, Modes.Measure));
        tagButton.onClick.AddListener(() => OnButtonClick(tagButton, Modes.Tag));
        clipButton.onClick.AddListener(() => OnButtonClick(clipButton, Modes.Clip)); 
        navigateButton.onClick.AddListener(() => OnButtonClick(navigateButton, Modes.Navigate));
        dicomButton.onClick.AddListener(() => OnDicomButtonClick());
        
        XRIDefaultInputActions inputAction = InputActionsManager.Instance.InputActions;
        inputAction.XRIRightHand.MenuButton.performed += OnMenuButtonPerformed;

        dicomImageUI.SetActive(false);
        Hide();
    }

    private void OnDicomButtonClick()
    {
        currentMode = Modes.Dicom;
        dicomImageUI.SetActive(true);
    }

    private void OnMenuButtonPerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is not TapInteraction) {
            return;
        }

        if (dicomImageUI.activeSelf || measurementToolsUI.activeSelf) 
        {
            dicomImageUI.SetActive(false);
            measurementToolsUI.SetActive(false);
            return;
        } else if (currentNavigation == Navigation.Outside) {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    private void OnButtonClick(Button clickedButton, Modes mode)
    {
        currentMode = mode;
        clickedButton.Select();

        if(mode == Modes.Navigate) { 
            OnNavigateEvent?.Invoke(this, EventArgs.Empty);
        }

        if (mode == Modes.Measure) {
            measurementToolsUI.SetActive(true);
        } else { 
            measurementToolsUI.SetActive(false);
        }
        Debug.Log("Current Mode: " + mode.ToString());
        Hide();
    }

    public Modes GetMode()
    {
        return currentMode;
    }

    public void SetMode(Modes mode) {
        currentMode = mode;
    }

    public Navigation GetNavigation()
    {
        return currentNavigation;
    }

    public void SetNavigation(Navigation navigation) {
        currentNavigation = navigation;
    }

    public void Hide() { 
        gameObject.SetActive(false);
    }
}
