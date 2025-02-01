using UnityEngine;

public class SpiderFollowAfterCollision : MonoBehaviour
{
    private Transform player; // Reference to the player's transform
    public float followSpeed = 3f; // Speed at which the spider follows the player
    public float stopDistance = 1.5f; // Minimum distance before stopping movement
    private bool isFollowing = false; // Whether the spider is currently following the player

    void Start()
    {
        // Automatically find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player is tagged as 'Player'.");
        }
    }

    void Update()
    {
        if (isFollowing && player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Move the spider towards the player only if it's outside the stopDistance
            if (distanceToPlayer > stopDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * followSpeed * Time.deltaTime;

                // Rotate spider to face the movement direction (similar to the player)
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    // Detect collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with the spider!");
            isFollowing = true; // Start following the player
        }
    }

    // Alternatively, use this if you want to detect trigger collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player triggered the spider!");
            isFollowing = true; // Start following the player
        }
    }
}
