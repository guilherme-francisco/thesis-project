using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    [SerializeField] private Button spriteTypeMeasurementButton;
    [SerializeField] private Button sphereMeasurementTypeButton;

    private void Awake() {
        spriteTypeMeasurementButton.onClick.AddListener(() => {
            MeasurementManager.Instance.SetCurrentVolumeMeasurementMethod(
                MeasurementManager.VolumeMeasurementMethods.Prefab
            );
            Hide();
        });

        sphereMeasurementTypeButton.onClick.AddListener(() => {
            MeasurementManager.Instance.SetCurrentVolumeMeasurementMethod(
                MeasurementManager.VolumeMeasurementMethods.Sphere
            );

            Hide();
        });


        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
