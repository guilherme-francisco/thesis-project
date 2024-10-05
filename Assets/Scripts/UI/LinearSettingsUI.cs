using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearSettingsUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button sphereMeasurementTypeButton;
    [SerializeField] private Button twoHandsMeasurementTypeButton; 
    [SerializeField] private Button handPositionMeasurementTypeButton;


    private void Awake() {
        sphereMeasurementTypeButton.onClick.AddListener(() => {
            MeasurementManager.Instance.SetCurrentLinearMeasurementMethod(
                MeasurementManager.LinearMeasurementMethods.Sphere
            );

            Hide();
        });

        twoHandsMeasurementTypeButton.onClick.AddListener(() => {
            MeasurementManager.Instance.SetCurrentLinearMeasurementMethod(
                MeasurementManager.LinearMeasurementMethods.TwoHands
            );

            Hide();
        });

        handPositionMeasurementTypeButton.onClick.AddListener(() => {
            MeasurementManager.Instance.SetCurrentLinearMeasurementMethod(
                MeasurementManager.LinearMeasurementMethods.HandPosition
            );

            Hide();
        });

        Hide();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
