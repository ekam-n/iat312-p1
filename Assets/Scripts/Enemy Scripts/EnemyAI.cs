using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 1f;
    public float visionAngle = 90f;
    public LayerMask obstacleLayer;

    protected Transform player;
    private bool isChasing = false;
    private IPatrol patrolBehavior;
    private LineRenderer visionLine;
    private Animator animator;

    public static bool showVisionCones = false; // Global toggle for vision cones

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player GameObject has the 'Player' tag.");
        }

        patrolBehavior = GetComponent<IPatrol>();

        // Setup LineRenderer for Vision Cone
        visionLine = GetComponent<LineRenderer>();
        if (visionLine == null)
        {
            visionLine = gameObject.AddComponent<LineRenderer>();
        }

        visionLine.positionCount = 3;
        visionLine.startWidth = 0.05f * chaseRange; // Scale based on chase range
        visionLine.endWidth = 0.05f * chaseRange;   // Scale based on chase range
        visionLine.material = new Material(Shader.Find("Sprites/Default"));
        visionLine.startColor = Color.yellow;
        visionLine.endColor = Color.yellow;

        animator = GetComponent<Animator>();
    }

    void Update()
{
    if (player == null) return;

    bool playerInVisionCone = IsPlayerInVisionCone() && !IsPlayerBlockedByObstacle();

    if (playerInVisionCone)
    {
        isChasing = true;
        animator.SetBool("isChasing", true);
    }
    else if (Vector2.Distance(transform.position, player.position) > chaseRange * 1.5f)
    {
        isChasing = false;
        animator.SetBool("isChasing", false);

    }

    if (isChasing)
    {
        ChasePlayer();
    }
    else if (patrolBehavior != null)
    {
        patrolBehavior.Patrol();
    }

    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    // Attack if close to the player
    if (distanceToPlayer < attackRange)
    {
        Attack();
    }

    // Rotate to face movement direction
    RotateTowardsMovement();
    
    // Update Vision Cone Visibility
    DrawVisionConeInGame();
}

void ChasePlayer()
{
    Vector2 direction = (player.position - transform.position).normalized;
    transform.position += (Vector3)direction * speed * Time.deltaTime;
}

void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        Debug.Log(gameObject.name + " touched the player! Player should die.");
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.KillPlayer();
        }
    }
}


void RotateTowardsMovement()
{
    if (isChasing && player != null)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Ensure vision cone updates correctly after rotation
    DrawVisionConeInGame();
}




    bool IsPlayerInVisionCone()
{
    Vector2 directionToPlayer = (player.position - transform.position).normalized;
    
    // Use transform.up instead of transform.right because the wasp faces upward
    float angleToPlayer = Vector2.Angle(transform.up, directionToPlayer);

    return angleToPlayer < visionAngle / 2 && Vector2.Distance(transform.position, player.position) <= chaseRange;
}


    bool IsPlayerBlockedByObstacle()
{
    Vector2 directionToPlayer = (player.position - transform.position).normalized;
    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);
    
    return hit.collider != null; // If it hits an obstacle, vision is blocked
}


    public virtual void Attack()
    {
        Debug.Log(gameObject.name + " is attacking with melee!");
    }

    void DrawVisionConeInGame()
{
    visionLine.enabled = showVisionCones; // Toggle visibility

    if (!showVisionCones) return; // If vision cones are disabled, exit

    Vector3 visionOrigin = transform.position; // Use transform.position to match detection logic
    Vector3 leftBoundary = Quaternion.Euler(0, 0, -visionAngle / 2) * transform.up * chaseRange;
    Vector3 rightBoundary = Quaternion.Euler(0, 0, visionAngle / 2) * transform.up * chaseRange;

    visionLine.SetPosition(0, visionOrigin);
    visionLine.SetPosition(1, visionOrigin + leftBoundary);
    visionLine.SetPosition(2, visionOrigin + rightBoundary);
}





}
