using System;
using UnityEngine;
using UnityEditor;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class FocusCameraShading : MonoBehaviour
{
    /// <summary>
    /// Post processing effect. This will probably handle all the post processing effects in the future.
    /// </summary>
    public Material material;
    //other focus shader
    public Material focusSquaresMaterial;
    ///Maybe do something similar to the ones below.
    [Tooltip("Intensity of effect.")]
    public float intensity = 0;
    //No need to store these in the camera. Also unity doesn't know how to display these completely normal floats :(
    public float vignetteRadius { get { return material.GetFloat("_VignetteRadius"); } set { material.SetFloat("_VignetteRadius", value); } }
    public float vignetteSmoothing { get { return material.GetFloat("_VignetteSmoothing"); } set { material.SetFloat("_VignetteSmoothing", value); } }

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
        material.SetFloat("_Intensity", intensity);//for some reason crashes if I pass it directly to shader
        focusSquaresMaterial.SetFloat("_Intensity", intensity);
        Graphics.Blit(source, destination, material);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(FocusCameraShading))]
public class RandomScript_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FocusCameraShading script = (FocusCameraShading)target;

        //Gotta do this to show the vignette tuning floats.
        script.vignetteRadius = EditorGUILayout.FloatField(new GUIContent("VignetteRadius", "Distance from center where intensity is max.\nIntended range 0.0 - 1.0"), script.vignetteRadius);
        script.vignetteSmoothing = EditorGUILayout.FloatField(new GUIContent("VignetteSmoothing", "Offset from vignette radius the intensity is smoothed over.\nIntended range 0.0 - 1.0"), script.vignetteSmoothing);
    }
}
#endif