using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private float currentSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0; // Ensure gravity is disabled for top-down movement
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");  // A/D or Left/Right Arrow
        moveInput.y = Input.GetAxisRaw("Vertical");    // W/S or Up/Down Arrow
        moveInput.Normalize(); // Prevent diagonal movement from being faster

        currentSpeed = moveInput.sqrMagnitude;

        animator.SetFloat("Speed", currentSpeed);
    }

    void FixedUpdate()
{
    if (moveInput.magnitude > 0) // Only rotate if moving
    {
        float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle; // Rotate player to face movement direction
    }

    rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
}

}
