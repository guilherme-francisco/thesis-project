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

    [SerializeField] private TextMeshProUGUI measurementValueText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;
    private float measurementValue;

    public event EventHandler<onSaveMeasurementEventArgs> onSaveMeasurementEvent;
    public class onSaveMeasurementEventArgs : EventArgs
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
        acceptButton.onClick.AddListener(handleAcceptButtonEvent);
        rejectButton.onClick.AddListener(() => { Hide(); });

        MeasurementManager.Instance.OnMeasurementEvent += HandleMeasurementEvent;
        Debug.Log("Subscription to OnMeasurementEvent");
        Hide();
    }

    private void handleAcceptButtonEvent()
    {
        onSaveMeasurementEvent?.Invoke(this, new onSaveMeasurementEventArgs
        {
            measurementValue = measurementValue
        });

        Hide();
    }

    private void HandleMeasurementEvent(object sender, MeasurementManager.OnMeasurementEventArgs args)
    {
        measurementValue = args.measurementValue;

        Debug.Log("Received measurement value: " + measurementValue);
            
        measurementValueText.text = measurementValue.ToString("F2") + " m";

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
