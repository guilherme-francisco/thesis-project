using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasurementToolsUI : MonoBehaviour
{
    public MeasurementToolsUI Instance { get; private set; }   
    public enum MeasurementTypes {
        Default,
        Linear,
        Curved,
        Radius,
        Volume
    }

    [SerializeField] private Button linearMeasurementButton;
    [SerializeField] private Button curvedMeasurementButton;
    [SerializeField] private Button radiusMeasurementButton;
    [SerializeField] private Button volumeMeasurementButton;


    private MeasurementTypes measurementType = MeasurementTypes.Default;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        linearMeasurementButton.onClick.AddListener(() => OnButtonClick(MeasurementTypes.Linear));
        curvedMeasurementButton.onClick.AddListener(() => OnButtonClick(MeasurementTypes.Curved));
        radiusMeasurementButton.onClick.AddListener(() => OnButtonClick(MeasurementTypes.Radius));
        volumeMeasurementButton.onClick.AddListener(() => OnButtonClick(MeasurementTypes.Volume));

        Hide();
    }

    private void OnButtonClick (MeasurementTypes measurementTypes) {
        measurementType = measurementTypes;
        Debug.Log("Measurement Type " + measurementType.ToString());
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
