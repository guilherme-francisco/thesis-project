using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SavedMeasurementPanelUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] Transform savedMeasurementSinglePanelUI;

    private List<float> SavedMeasuramentValuesList;

    private void Awake()
    {
        savedMeasurementSinglePanelUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        SavedMeasuramentValuesList = new List<float>();
        MeasurementInformationUI.Instance.onSaveMeasurementEvent += MeasurementPanelUI_OnSaveMeasurament;
        Hide();
    }

    private void MeasurementPanelUI_OnSaveMeasurament(object sender, MeasurementInformationUI.onSaveMeasurementEventArgs e)
    {
        // Save measurement
        SavedMeasuramentValuesList.Add(e.measurementValue);
        
        // Destroy previous child
        foreach (Transform child in container)
        {
            if (child == savedMeasurementSinglePanelUI) continue;
            Destroy(child.gameObject);
        }

        // Show update visual for user
        foreach (var (measurementValue, index) in SavedMeasuramentValuesList.Select((value, i) => (value, i)))
        {
            Transform measurementMadeTransform = Instantiate(savedMeasurementSinglePanelUI, container);
            measurementMadeTransform.gameObject.SetActive(true);
            measurementMadeTransform.GetComponent<SavedMeasurementSinglePanelUI>().setText((index + 1).ToString(), 
                measurementValue.ToString("F2") + " m");
        }

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);   
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
