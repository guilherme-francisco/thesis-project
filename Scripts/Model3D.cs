using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Model3D : MonoBehaviour
{
    public string path = @"Assets/Data/";
    public string filename = "model";

    private void Start() {
                // Get the renderer component of the object
        Renderer renderer = GetComponentInChildren<Renderer>();

        // Check if the renderer component exists
        if (renderer != null)
        {
            // Get the main texture (albedo) of the object
            Texture texture = renderer.material.mainTexture;

            Debug.Log("Number of materials" +  renderer.materials.Length.ToString());

            // Use the main texture as needed
            if (texture != null)
            {
                // assign it to the material of the parent object
                GetComponentInChildren<Renderer>().sharedMaterial.SetTexture("_Data", texture);
                GetComponentInChildren<Renderer> ().sharedMaterial.SetFloat("_SliceAxis1Min", 0);
                GetComponentInChildren<Renderer> ().sharedMaterial.SetFloat ("_SliceAxis1Max", 1);
                GetComponentInChildren<Renderer> ().sharedMaterial.SetFloat("_SliceAxis2Min", 0);
                GetComponentInChildren<Renderer> ().sharedMaterial.SetFloat ("_SliceAxis2Max", 1);
                GetComponentInChildren<Renderer> ().sharedMaterial.SetFloat ("_SliceAxis3Min", 0);
                GetComponentInChildren<Renderer> ().sharedMaterial.SetFloat ("_SliceAxis3Max", 1);
                GetComponentInChildren<Renderer> ().sharedMaterial.SetFloat ("_DataMin", 0);
                GetComponentInChildren<Renderer> ().sharedMaterial.SetFloat ("_DataMax", 1);
                GetComponentInChildren<Renderer> ().sharedMaterial.SetFloat ("_Iterations", 2048);

                // save it as an asset for re-use
                #if UNITY_EDITOR
                AssetDatabase.CreateAsset (texture, path + filename + ".asset");
                #endif
            }
        }
    }
}