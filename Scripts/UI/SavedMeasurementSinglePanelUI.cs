using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavedMeasurementSinglePanelUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI index;
    [SerializeField] TextMeshProUGUI value;

    public void setText(string index, string value)
    {
        this.index.text = index;
        this.value.text = value;
    }
}
