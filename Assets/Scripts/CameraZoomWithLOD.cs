using UnityEngine;

public class CameraZoomWithLOD : MonoBehaviour
{
    public Camera mainCamera;                  // Reference to the camera
    public Transform target;                   // The target object
    public GameObject lowDetailModel;          // Low detail model (e.g., low-poly model)
    public GameObject mediumDetailModel;       // Medium detail model (e.g., medium-poly model)
    public GameObject highDetailModel;         // High detail model (e.g., high-poly model)

    public float zoomSpeed = 5f;               // Speed of the zoom
    public float minFov = 20f;                 // Minimum Field of View (zoomed in)
    public float maxFov = 60f;                 // Maximum Field of View (zoomed out)

    private float currentFov;

    void Start()
    {
        // Set the initial FOV
        currentFov = mainCamera.fieldOfView;
    }

    void Update()
    {
        // Get scroll input from the mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the field of view based on scroll input
        currentFov -= scroll * zoomSpeed * 100f * Time.deltaTime;

        // Clamp the FOV between the min and max values
        currentFov = Mathf.Clamp(currentFov, minFov, maxFov);

        // Apply the FOV to the camera
        mainCamera.fieldOfView = currentFov;

        // Update the LOD based on the current zoom level
        UpdateLOD();
    }

    private void UpdateLOD()
    {
        // Calculate distance between the camera and the target
        float distance = Vector3.Distance(mainCamera.transform.position, target.position);

        // Switch LOD based on the camera distance (zoom level)
        if (distance < 10f) // Close to the target, use high detail
        {
            SetLOD("High");
        }
        else if (distance < 20f) // Medium distance, use medium detail
        {
            SetLOD("Medium");
        }
        else // Far away, use low detail
        {
            SetLOD("Low");
        }
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
}
