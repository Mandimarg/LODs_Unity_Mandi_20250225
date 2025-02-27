using UnityEngine;

public class CameraZoomWithLODAnimation : MonoBehaviour
{
    public Camera mainCamera;                  // Reference to the camera
    public Transform target;                   // The target object (parent of LOD models)
    public GameObject lowDetailModel;          // Low detail model
    public GameObject mediumDetailModel;       // Medium detail model
    public GameObject highDetailModel;         // High detail model

    public float zoomSpeed = 2f;               // Speed of the camera animation
    public float[] zoomDistances = { 10f, 20f, 30f }; // Distances for LOD changes (closer = higher detail)

    private int currentZoomLevel = 0;          // The current zoom level (index of the zoomDistances array)

    private void Start()
    {
        // Start with the first LOD model based on the initial distance
        UpdateLOD();
    }

    private void Update()
    {
        // Move the camera forward and backward using keys or input (e.g., W and S for forward/backward)
        if (Input.GetKey(KeyCode.W))
        {
            // Move camera forward (closer to the target)
            AnimateCameraMovement(zoomDistances[currentZoomLevel + 1]);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Move camera backward (away from the target)
            AnimateCameraMovement(zoomDistances[currentZoomLevel - 1]);
        }

        // Switch between LODs based on camera distance
        float distance = Vector3.Distance(mainCamera.transform.position, target.position);
        if (distance < zoomDistances[1]) SetLOD("High");
        else if (distance < zoomDistances[2]) SetLOD("Medium");
        else SetLOD("Low");
    }

    private void AnimateCameraMovement(float targetDistance)
    {
        // Animate the camera moving smoothly toward the target distance
        StartCoroutine(MoveCameraSmoothly(targetDistance));
    }

    private System.Collections.IEnumerator MoveCameraSmoothly(float targetDistance)
    {
        // Get the target position based on the zoom level
        Vector3 targetPosition = target.position + (mainCamera.transform.position - target.position).normalized * targetDistance;

        // Animate camera position smoothly towards target position
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
    }

    private void SetLOD(string level)
    {
        // Disable all models first
        lowDetailModel.SetActive(false);
        mediumDetailModel.SetActive(false);
        highDetailModel.SetActive(false);

        // Enable the appropriate model based on the LOD level
        switch (level)
        {
            case "Low":
                lowDetailModel.SetActive(true);
                break;
            case "Medium":
                mediumDetailModel.SetActive(true);
                break;
            case "High":
                highDetailModel.SetActive(true);
                break;
        }
    }

    private void UpdateLOD()
    {
        // Set LOD based on the initial camera distance
        float distance = Vector3.Distance(mainCamera.transform.position, target.position);
        if (distance < zoomDistances[0])
        {
            SetLOD("High");
        }
        else if (distance < zoomDistances[1])
        {
            SetLOD("Medium");
        }
        else
        {
            SetLOD("Low");
        }
    }
}

