using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UIs")]
    [SerializeField] private GameObject linearSettingsUI;
    [SerializeField] private GameObject curvedSettingsUI;
    [SerializeField] private GameObject radiusSettingsUI;

    [SerializeField] private GameObject volumeSettingsUI;
    
    [Header("Buttons")]
    [SerializeField] private Button linearSettingsButton;
    [SerializeField] private Button curvedSettingsButton;
    [SerializeField] private Button radiusSettingsButton;

    [SerializeField] private Button volumeSettingsButton;

    private void Awake() {
        linearSettingsButton.onClick.AddListener(() => {
            linearSettingsUI.SetActive(true);
            Hide();
        });

        curvedSettingsButton.onClick.AddListener(() => {
            curvedSettingsUI.SetActive(true);
            Hide();
        });

        radiusSettingsButton.onClick.AddListener(() => {
            radiusSettingsUI.SetActive(true);
            Hide();
        });

        volumeSettingsButton.onClick.AddListener(() => {
            volumeSettingsUI.SetActive(true);
            Hide();
        });

        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
