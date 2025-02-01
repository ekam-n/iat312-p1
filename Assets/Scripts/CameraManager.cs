using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public Transform[] cameraPositions; // Assign preset positions in Inspector
    public float[] zoomLevels; // Corresponding zoom levels for each zone
    public float transitionSpeed = 1.5f; // Adjust transition speed
    public float zoomSpeed = 1f; // Adjust zoom speed

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (cameraPositions.Length > 0)
        {
            // Start at the first zone's camera position
            mainCamera.transform.position = cameraPositions[0].position;
            mainCamera.orthographicSize = zoomLevels[0]; 
        }
    }

    public void MoveToNextZone(int zoneIndex)
    {
        if (zoneIndex >= 0 && zoneIndex < cameraPositions.Length)
        {
            StartCoroutine(SmoothCameraTransition(cameraPositions[zoneIndex].position, zoomLevels[zoneIndex]));
        }
    }

    private IEnumerator SmoothCameraTransition(Vector3 newPosition, float targetZoom)
    {
        float elapsedTime = 0f;
        Vector3 startPos = mainCamera.transform.position;
        float startZoom = mainCamera.orthographicSize;

        while (elapsedTime < transitionSpeed)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, newPosition, elapsedTime / transitionSpeed);
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = newPosition;
        mainCamera.orthographicSize = targetZoom;
    }

    public void MoveCameraToPosition(Vector3 position)
{
    StartCoroutine(SmoothCameraTransition(position, mainCamera.orthographicSize));
}

}
