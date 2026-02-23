using UnityEngine;
using System;

public class BackgroundAnim : MonoBehaviour
{
    [Tooltip("Degrees per second")]
    private float rotationSpeed = 10f;
    private float currentRotation = 0f;
    private float currentMoonHalo = 1f;
    private float time = 0f;

    void Update()
    {
        currentRotation = (currentRotation + rotationSpeed * Time.deltaTime) % 360f;
        time += Time.deltaTime;
        currentMoonHalo = (float)Math.Sin(time);
        RenderSettings.skybox.SetFloat("_CloudsRotation", currentRotation);
        RenderSettings.skybox.SetFloat("_MoonHalo", currentMoonHalo);
    }

}