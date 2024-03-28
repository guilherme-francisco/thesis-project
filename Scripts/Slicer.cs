using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{

    [SerializeField] private Material volumetricMaterial;

    public void OnXSliderValueChanged(float value)
    {
        Debug.Log("XSlider changed: " + value);
        volumetricMaterial.SetFloat("_SliceAxis1Min", value);
        //volumetricMaterial.SetFloat("_SliceAxis1Max", 1 - value);
    }

    public void OnYSliderValueChanged(float value)
    {
        Debug.Log("YSlider changed: " + value);
        volumetricMaterial.SetFloat("_SliceAxis2Min", value);
        //volumetricMaterial.SetFloat("_SliceAxis2Max", 1 - value);
    }

    public void OnZSliderValueChanged(float value)
    {
        Debug.Log("ZSlider changed: " + value);
        volumetricMaterial.SetFloat("_SliceAxis3Min", value);
        //volumetricMaterial.SetFloat("_SliceAxis3Max", 1 - value);
    }
}
