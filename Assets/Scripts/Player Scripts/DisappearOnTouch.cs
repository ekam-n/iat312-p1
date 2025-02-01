using UnityEngine;

public class SpiderFollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float followSpeed = 3f; // Speed at which the spider follows the player
    public float detectionRange = 2f; // Range within which the spider detects the player

    private bool isFollowing = false; // Whether the spider is currently following the player

    void Update()
    {
        if (isFollowing)
        {
            // Move the spider towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        }
        else
        {
            // Check if the player is within detection range
            if (Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                isFollowing = true; // Start following the player
            }
        }
    }

    // Optional: Use OnTriggerEnter2D if you want the spider to follow when touched
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            isFollowing = true; // Start following the player
        }
    }
}