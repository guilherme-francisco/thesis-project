using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Dicom;
using System.IO;
using Dicom.Imaging;
using TMPro;
using Dicom.Imaging.Render;

public class DicomImageUI : MonoBehaviour
{
    [SerializeField] private string folder;
    [SerializeField] private Transform axialImage;
    [SerializeField] private Transform coronalImage;
    [SerializeField] private Transform sagittalImage;
    [SerializeField] private TextMeshProUGUI axialValueText;
    [SerializeField] private TextMeshProUGUI coronalValueText;
    [SerializeField] private TextMeshProUGUI sagittalValueText;
    private int[] imageShape;

    private DicomUtils dicomUtils;
    private void Awake() {
        dicomUtils = new DicomUtils(folder);
        imageShape = dicomUtils.GetImageShape();
        Debug.Log("Image Shape: " + '(' + imageShape[0] + ',' + imageShape[1] + ',' +imageShape[2] + ')');
    }

    private void Start() {
        ChangeAxialSlice(0.5f);
        ChangeCoronalSlice(0.5f);
        ChangeSagittalSlice(0.5f);
    }
    
    public void ChangeAxialSlice(float slice) {
        axialImage.GetComponent<UnityEngine.UI.Image>().sprite = dicomUtils.GetSpriteFromTexture(dicomUtils.GetAxialSliceTexture((int)(slice * imageShape[2])));
        axialValueText.text = Convert.ToInt32(imageShape[2] *slice).ToString() + "/" + imageShape[2].ToString();
    }

    public void ChangeCoronalSlice(float slice) {
        coronalImage.GetComponent<UnityEngine.UI.Image>().sprite = dicomUtils.GetSpriteFromTexture(dicomUtils.GetCoronalSliceTexture((int)(imageShape[0] * slice)));
        coronalValueText.text = Convert.ToInt32(imageShape[0] * slice).ToString() + "/" + imageShape[0].ToString();
    }

    public void ChangeSagittalSlice(float slice) {
        sagittalImage.GetComponent<UnityEngine.UI.Image>().sprite = dicomUtils.GetSpriteFromTexture(dicomUtils.GetSagittalSliceTexture((int)(imageShape[1] * slice)));
        sagittalValueText.text = Convert.ToInt32(imageShape[1] * slice).ToString() + "/" + imageShape[1].ToString();
    }
}
