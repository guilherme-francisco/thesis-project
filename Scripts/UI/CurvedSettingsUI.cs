using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurvedSettingsUI : MonoBehaviour
{
    [SerializeField] private Button sphereMeasurementTypeButton;

    [SerializeField] private Button handPositionMeasurementTypeButton;


    
    private void Awake() {
        sphereMeasurementTypeButton.onClick.AddListener(() => {
            MeasurementManager.Instance.SetCurrentCurvedMeasurementMethod(
                MeasurementManager.CurvedMeasurementMethods.Sphere
            );

            Hide();
        });

        handPositionMeasurementTypeButton.onClick.AddListener(() => {
            MeasurementManager.Instance.SetCurrentCurvedMeasurementMethod(
                MeasurementManager.CurvedMeasurementMethods.HandPosition
            );

            Hide();
        });

        Hide();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

}
