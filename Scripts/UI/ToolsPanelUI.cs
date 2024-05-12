using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsPanelUI : MonoBehaviour
{
    public event EventHandler OnNavigateEvent;

    [SerializeField] private GameObject clippingPanelUI;

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
        Navigate
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

    private Modes currentMode = Modes.Default;

    private Navigation currentNavigation = Navigation.Outside;

    private void Awake()
    {
        Instance = this;
        clippingPanelUI.SetActive(false);
    }

    private void Start()
    {
        // Add listeners to each button
        moveButton.onClick.AddListener(() => OnButtonClick(moveButton, Modes.Move));
        rotateButton.onClick.AddListener(() => OnButtonClick(rotateButton, Modes.Rotate));
        scaleButton.onClick.AddListener(() => OnButtonClick(scaleButton, Modes.Scale));
        measureButton.onClick.AddListener(() => OnButtonClick(measureButton, Modes.Measure));
        tagButton.onClick.AddListener(() => OnButtonClick(tagButton, Modes.Tag));
        clipButton.onClick.AddListener(() => OnButtonClick(clipButton, Modes.Clip)); 
        navigateButton.onClick.AddListener(() => OnButtonClick(navigateButton, Modes.Navigate));
    }
    
    private void OnButtonClick(Button clickedButton, Modes mode)
    {
        currentMode = mode;
        clickedButton.Select();

        if (mode == Modes.Clip)
        {
            clippingPanelUI.SetActive(true);
        } else { 
            clippingPanelUI.SetActive(false);
        }

        if(mode == Modes.Navigate) { 
            OnNavigateEvent?.Invoke(this, EventArgs.Empty);
        }

        if (mode == Modes.Measure) {
            measurementToolsUI.SetActive(true);
        } else { 
            measurementToolsUI.SetActive(false);
        }

        Debug.Log("Current Mode: " + mode.ToString());
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
}
