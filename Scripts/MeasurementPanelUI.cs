using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeasurementPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI measurementValueText;

    private void Start() {
        SphereCreator.Instance.OnMeasurementEvent += HandleMeasurementEvent;
        Debug.Log("Subscription to OnMeasurementEvent");
        Hide();
    }

    private void HandleMeasurementEvent(object sender, SphereCreator.OnMeasurementEventArgs args)
    {
        float measurementValue = args.measurementValue;

        Debug.Log("Received measurement value: " + measurementValue);
            
        measurementValueText.text = measurementValue.ToString("F2") + " m";

        Show();
    }

     private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
