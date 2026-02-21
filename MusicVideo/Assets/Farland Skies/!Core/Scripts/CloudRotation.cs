using UnityEngine;

public class CloudRotation : MonoBehaviour
{
    [Tooltip("Degrees per second")]
    public float rotationSpeed = 10f;

    private float currentRotation = 0f;

    void Update()
    {
        currentRotation = (currentRotation + rotationSpeed * Time.deltaTime) % 360f;
        RenderSettings.skybox.SetFloat("_CloudsRotation", currentRotation);
    }
}