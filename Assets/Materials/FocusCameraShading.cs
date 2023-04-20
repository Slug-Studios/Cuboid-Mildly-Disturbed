using System;
using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class FocusCameraShading : MonoBehaviour
{

    public Material material;
    public float intensity = 0;

    void Start()
    {
        if (null == material || null == material.shader ||
           !material.shader.isSupported)
        {
            enabled = false;
            return;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Intensity", intensity);
        Graphics.Blit(source, destination, material);
    }
}