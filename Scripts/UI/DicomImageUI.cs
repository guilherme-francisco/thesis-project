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

    private List<DicomFile> dicomArray;

    Texture3D texture3D;

    int[] imageShape;

    private Sprite GetSpriteFromTexture(Texture2D texture) {
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Vector4 border = Vector4.zero; 
    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, 1f, 0, SpriteMeshType.FullRect, border);    }


    private List<DicomFile> ImportDicomImages(string folder)
    {
        List<DicomFile> files = new List<DicomFile>();

        foreach (string fileName in Directory.EnumerateFiles(folder))
        {
            if (Path.GetExtension(fileName).ToLower() == ".dcm")
            {
                files.Add(DicomFile.Open(fileName));
            }
        }

        List<DicomFile> slices = new List<DicomFile>();
        int skipCount = 0;

        foreach (DicomFile file in files)
        {
            if (file.Dataset.Contains(DicomTag.SliceLocation))
            {
                slices.Add(file);
            }
            else
            {
                skipCount++;
            }
        }

        Debug.Log($"Skipped, no SliceLocation: {skipCount}");

        return slices.OrderBy(s => s.Dataset.Get<double>(DicomTag.SliceLocation)).ToList();
    }

    private void GetTexture3D (List<DicomFile> dicomArray){
        int rows = dicomArray[0].Dataset.Get<int>(DicomTag.Rows);
        int columns = dicomArray[0].Dataset.Get<int>(DicomTag.Columns);
        int numSlices = dicomArray.Count;

        Debug.Log("Number of Rows: " + rows);
        Debug.Log("Number of Columns: " + columns);
        Debug.Log("Number of Slices: " + numSlices);

        imageShape = new int[] { rows, columns, numSlices };

        Color[] voxelColors = new Color[rows * columns * numSlices];

        for (int i = 0; i < numSlices; i++)
        {
            var image = new DicomImage(dicomArray[i].Dataset);
            var texture = image.RenderImage().AsTexture2D();

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    int index = col + row * columns + i * columns * rows;
                    voxelColors[index] = texture.GetPixel(row, col);
                }
            }
        }

        texture3D = new Texture3D(rows, columns, numSlices, TextureFormat.RGBA32, false);
        texture3D.SetPixels(voxelColors);
        texture3D.Apply();

    }

    private Texture2D GetSagittalSliceTexture(int sliceIndex)
    {
        int height = texture3D.height;
        int depth = texture3D.depth;

        Color[] pixels = new Color[height * depth];

        for (int x = 0; x < height; x++){
            for(int z = 0; z < depth; z++) {
                pixels[x * depth + z] = texture3D.GetPixel(x, sliceIndex, z);
            }
        }

        Texture2D sliceTexture = new Texture2D(depth, height, TextureFormat.RGBA32, false);
        sliceTexture.SetPixels(pixels);
        sliceTexture.Apply();

        return sliceTexture;
    }


    private Texture2D GetCoronalSliceTexture(int sliceIndex)
    {
        int width = texture3D.width;
        int depth = texture3D.depth;

        Color[] pixels = new Color[width * depth];

        
        for (int z = 0; z < depth; z++)
        {
            for (int y = 0; y < width; y++)
            {
                pixels[z * width + y] = texture3D.GetPixel(sliceIndex, y, depth - z - 1);
            }
        }


        Texture2D sliceTexture = new Texture2D(width, depth, TextureFormat.RGBA32, false);
        sliceTexture.SetPixels(pixels);
        sliceTexture.Apply();

        return sliceTexture;
    }

    private Texture2D GetAxialSliceTexture(int sliceIndex)
    {
        int width = texture3D.width;
        int height = texture3D.height;

        Color[] pixels =  new Color[width * height];


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y< height; y++)
            {
                pixels[x * height + y] = texture3D.GetPixel(x, y, sliceIndex);
            }
        }

        Texture2D sliceTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        sliceTexture.SetPixels(pixels);
        sliceTexture.Apply();

        return sliceTexture;
    }

    private void Awake() {
        dicomArray = ImportDicomImages(folder);
        GetTexture3D(dicomArray); 
        
    }


    private void Start() {
        coronalImage.GetComponent<UnityEngine.UI.Image>().sprite = GetSpriteFromTexture(GetCoronalSliceTexture(imageShape[0] / 2));
        axialImage.GetComponent<UnityEngine.UI.Image>().sprite = GetSpriteFromTexture(GetAxialSliceTexture(imageShape[2] / 2));
        sagittalImage.GetComponent<UnityEngine.UI.Image>().sprite = GetSpriteFromTexture(GetSagittalSliceTexture(imageShape[1] / 2));

        coronalValueText.text = (imageShape[0] / 2).ToString() + "/" + imageShape[0].ToString();
        sagittalValueText.text = (imageShape[1] / 2).ToString() + "/" + imageShape[1].ToString();
        axialValueText.text = (imageShape[2] / 2).ToString() + "/" + imageShape[2].ToString();
    }
}
