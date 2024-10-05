using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadiusSettingsUI : MonoBehaviour
{
    [SerializeField] private Button spriteTypeMeasurementButton;
    [SerializeField] private Button sphereMeasurementTypeButton;

    private void Awake() {
        spriteTypeMeasurementButton.onClick.AddListener(() => {
            MeasurementManager.Instance.SetCurrentRadiusMeasurementMethod(
                MeasurementManager.RadiusMeasurementMethods.Prefab
            );
            Hide();
        });

        sphereMeasurementTypeButton.onClick.AddListener(() => {
            MeasurementManager.Instance.SetCurrentRadiusMeasurementMethod(
                MeasurementManager.RadiusMeasurementMethods.Sphere
            );

            Hide();
        });


        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
