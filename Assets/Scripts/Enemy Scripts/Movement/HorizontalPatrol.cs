using System.Collections;
using UnityEngine;

public class HorizontalPatrol : MonoBehaviour, IPatrol
{
    public float speed = 2f;
    public float patrolDistance = 3f;
    public float patrolOffset = 0f; // Controlled offset (set in Inspector)

    private Vector2 startPosition;
    private int direction = 1; // 1 = right, -1 = left
    private bool waiting = false;

    void Start()
    {
        startPosition = transform.position;
        transform.position = new Vector3(startPosition.x + patrolOffset * direction, startPosition.y, transform.position.z);
    }

    public void Patrol()
    {
        if (waiting) return; // If enemy is waiting, do nothing

        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);

        if (Mathf.Abs(transform.position.x - startPosition.x) >= patrolDistance)
        {
            StartCoroutine(PauseBeforeTurning());
        }

        // Adjust rotation to face movement direction
        float angle = direction == 1 ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private IEnumerator PauseBeforeTurning()
    {
        waiting = true;
        yield return new WaitForSeconds(0f); // Pause for half a second before turning

        // **Snap to exact patrol position before switching direction**
        transform.position = new Vector2(startPosition.x + patrolDistance * direction, transform.position.y);

        direction *= -1;
        waiting = false;
    }
}
