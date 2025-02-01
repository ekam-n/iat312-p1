using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public int zoneIndex; // Assign in Inspector
    public bool isSafeZone = false; // Mark in Inspector if this is a safe zone

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager cameraManager = Camera.main.GetComponent<CameraManager>();
            if (cameraManager != null)
            {
                cameraManager.MoveToNextZone(zoneIndex);
            }

            // If this is a safe zone, update the player's spawn point with the correct camera zone
            if (isSafeZone)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.SetSpawnPoint(transform.position, zoneIndex);
                }
            }
        }
    }
}
