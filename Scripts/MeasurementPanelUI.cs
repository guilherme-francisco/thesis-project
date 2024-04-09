using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MeasurementPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI measurementValueText;
    [SerializeField] private Button accept;
    [SerializeField] private Button reject;

    private void Start() {
        GameManager.Instance.OnMeasurementEvent += HandleMeasurementEvent;
        Debug.Log("Subscription to OnMeasurementEvent");

        accept.onClick.AddListener(() => { Hide(); });
        reject.onClick.AddListener(() => { Hide(); });

        Hide();
    }

    private void HandleMeasurementEvent(object sender, GameManager.OnMeasurementEventArgs args)
    {
        float measurementValue = args.measurementValue;

        Debug.Log("Received measurement value: " + measurementValue);
            
        measurementValueText.text = measurementValue.ToString("F2") + " m";

        Show();
    }

     private void Show() {
        gameObject.SetActive(true);
        accept.gameObject.SetActive(true);
        accept.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
