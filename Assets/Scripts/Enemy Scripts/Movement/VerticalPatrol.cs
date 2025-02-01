using UnityEngine;
using System.Collections;

public class VerticalPatrol : MonoBehaviour, IPatrol
{
    public float speed = 2f;
    public float patrolDistance = 3f;
    public float patrolOffset = 0f; // Controlled offset (set in Inspector)

    private Vector2 startPosition;
    private int direction = 1; // 1 = up, -1 = down
    private bool waiting = false;

    void Start()
    {
        startPosition = transform.position; // Store original position

        // Apply manual offset so enemies start staggered
        transform.position = new Vector3(transform.position.x, startPosition.y + patrolOffset * direction, transform.position.z);
    }

    public void Patrol()
    {
        if (waiting) return; // If enemy is waiting, do nothing

        transform.position += new Vector3(0, speed * direction * Time.deltaTime, 0);

        if (Mathf.Abs(transform.position.y - startPosition.y) >= patrolDistance)
        {
            StartCoroutine(PauseBeforeTurning());
        }

        // Adjust rotation to face movement direction
        float angle = direction == 1 ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private IEnumerator PauseBeforeTurning()
    {
        waiting = true;
        yield return new WaitForSeconds(0f); // Pause for half a second before turning

        // **Snap to exact patrol position before switching direction**
        transform.position = new Vector2(transform.position.x, startPosition.y + patrolDistance * direction);

        direction *= -1;
        waiting = false;
    }
}
