using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject ClippingPanelUI;

    public static ToolsPanelUI Instance { get; private set; }

    public enum Modes
    {
        Default,
        Move,
        Rotate,
        Scale,
        Measure,
        Tag,
        Clip
    }

    [SerializeField] private Button moveButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button scaleButton;
    [SerializeField] private Button measureButton;
    [SerializeField] private Button tagButton;
    [SerializeField] private Button clipButton;

    private Modes currentMode = Modes.Default;

      private void Awake()
    {
        Instance = this;
        ClippingPanelUI.SetActive(false);
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
    }
    
    private void OnButtonClick(Button clickedButton, Modes mode)
    {
        currentMode = mode;
        clickedButton.Select();

        if (mode == Modes.Clip)
        {
            ClippingPanelUI.SetActive(true);
        } else { 
            ClippingPanelUI.SetActive(false);
        }

        Debug.Log("Current Mode: " + mode.ToString());
    }

    public Modes GetMode()
    {
        return currentMode;
    }

}
