using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MeasurementInformationUI : MonoBehaviour
{
    public static MeasurementInformationUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI measurementLabelText;
    [SerializeField] private TextMeshProUGUI secondMeasurementLabelText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI measurementValueText;
    [SerializeField] private TextMeshProUGUI secondMeasurementValueText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;
    private float measurementValue;

    public event EventHandler<OnSaveMeasurementEventArgs> OnSaveMeasurementEvent;
    public class OnSaveMeasurementEventArgs : EventArgs
    {
        public float measurementValue;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate instance of MeasaurementPanelUI. Destroying the duplicate.");
            Destroy(gameObject);
        }

    }

    private void Start() {
        acceptButton.onClick.AddListener(HandleAcceptButtonEvent);
        rejectButton.onClick.AddListener(() => { Hide(); });

        MeasurementManager.Instance.OnMeasurementEvent += HandleMeasurementEvent;
        Debug.Log("Subscription to OnMeasurementEvent");
        Hide();
    }

    private void HandleAcceptButtonEvent()
    {
        OnSaveMeasurementEvent?.Invoke(this, new OnSaveMeasurementEventArgs
        {
            measurementValue = measurementValue
        });

        Hide();
    }

    private void HandleMeasurementEvent(object sender, MeasurementManager.OnMeasurementEventArgs args)
    {
        var description = "The size of the polygon is";
        var measurementLabel = "Virtual Scale: ";
        var secondMeasurementLabel = "Real Scale: ";
        var primaryScale = " m";
        var secondaryScale = " mm";

        switch (args.measurementTypes) 
        {
            case MeasurementToolsUI.MeasurementTypes.Curved:
            case MeasurementToolsUI.MeasurementTypes.Linear:
            break;
            case MeasurementToolsUI.MeasurementTypes.Radius:
            description = "The area/radius of the polygon is";
            measurementLabel = "Radius:";
            secondMeasurementLabel = "Area: ";
            secondaryScale = " m²";
            break;
            case MeasurementToolsUI.MeasurementTypes.Volume:
            description = "The volume of the polygon is";
            primaryScale = " m³";
            secondaryScale = " mm³";
            break;
            
        }

        descriptionText.text = description;
        measurementLabelText.text = measurementLabel;
        secondMeasurementLabelText.text = secondMeasurementLabel;
        
        measurementValue = args.measurementValue;

        Debug.Log("Received measurement value: " + measurementValue);
            
        measurementValueText.text = measurementValue.ToString("F2") + primaryScale;

       secondMeasurementValueText.text = args.secondMeasurementValue.ToString("F2") + secondaryScale;

        Show();
    }

     private void Show() {
        gameObject.SetActive(true);
        acceptButton.gameObject.SetActive(true);
        acceptButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
