using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHits = 2; // Player dies after 2 hits
    private int currentHits = 0;
    private Vector3 spawnPoint; // The player's last safe zone
    private int spawnZoneIndex = 0; // The camera zone index of the last safe zone

    public GameObject respawnButton; // Assign in Inspector
    private CameraManager cameraManager; // Reference to CameraManager

    void Start()
    {
        spawnPoint = transform.position; // Initial spawn point
        cameraManager = FindFirstObjectByType<CameraManager>(); // Get CameraManager

        if (respawnButton != null)
        {
            respawnButton.SetActive(false); // Hide respawn button initially
        }
    }

    public void TakeDamage()
    {
        currentHits++;
        Debug.Log("Player hit! Hits taken: " + currentHits);

        if (currentHits >= maxHits)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        Debug.Log("Player has been killed!");
        gameObject.SetActive(false); // Hide player instead of destroying
        if (respawnButton != null)
        {
            respawnButton.SetActive(true); // Show the respawn button
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true); // Reactivate player
        transform.position = spawnPoint; // Move to last safe zone
        currentHits = 0; // Reset health

        if (respawnButton != null)
        {
            respawnButton.SetActive(false); // Hide respawn button after respawning
        }

        // **Move the camera to the correct predefined zone**
        if (cameraManager != null)
        {
            cameraManager.MoveToNextZone(spawnZoneIndex);
        }

        // **Reset all enemies to their original positions**
        ResetAllEnemies();
    }

    private void ResetAllEnemies()
    {
        EnemyAI[] allEnemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        foreach (EnemyAI enemy in allEnemies)
        {
            enemy.ResetPosition();
        }
    }

    public void SetSpawnPoint(Vector3 newSpawn, int zoneIndex)
    {
        spawnPoint = newSpawn;
        spawnZoneIndex = zoneIndex;
        Debug.Log("Spawn point updated to: " + spawnPoint + " (Zone Index: " + spawnZoneIndex + ")");
    }
}
