using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float gravityScale = 3f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float playerScale = 4f;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider; 
    private float horizontalInput;
    private bool isSprinting;
    private Health health;

    [Header("Jump Sound")]
    [SerializeField] private AudioClip jumpSound;

    private const float flipThreshold = 0.01f; // Threshold to determine when to flip the player

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        health = GetComponent<Health>(); // Get the Health component
    }

    private void Update()
    {
        // Prevent movement and animations if the player is dead
        if (health.IsDead)
        {
            body.velocity = Vector2.zero; // Stop all movement
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("Grounded", true);
            return;
        }
        HandleInput();
        HandleMovement();
        HandleFlip();
        HandleAnimations();
    }

    private void HandleInput()
    {
        // Get horizontal input (A/D or Left/Right arrow keys)
        horizontalInput = Input.GetAxis("Horizontal");

        // Check if the sprint key (Left Shift) is being held
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Check if the jump key (W) is pressed and the player is grounded
        if (Input.GetKey(KeyCode.W) && isGrounded())
        {
            Jump();

            if (Input.GetKeyDown(KeyCode.W) && isGrounded())
            {
                SoundManager.instance.PlaySound(jumpSound);
            }
        }
    }

    private void HandleMovement()
    {
        // Determine movement speed (increased when sprinting)
        float moveSpeed = isSprinting ? speed * sprintMultiplier : speed;

        // Apply horizontal velocity while maintaining current vertical velocity
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);

        // Apply default gravity
        body.gravityScale = gravityScale;
    }

    private void HandleFlip()
    {
        // Flip the sprite to the right if moving right
        if (horizontalInput > flipThreshold)
        {
            transform.localScale = new Vector3(playerScale, playerScale, playerScale);
        }
        // Flip the sprite to the left if moving left
        else if (horizontalInput < -flipThreshold)
        {
            transform.localScale = new Vector3(-playerScale, playerScale, playerScale);
        }
    }

    private void HandleAnimations()
    {
        anim.SetBool("Walk", Mathf.Abs(horizontalInput) > flipThreshold);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("Run", isSprinting);
    }

    private void Jump()
    {
        // Apply jump force along the y-axis
        anim.SetTrigger("Jump");
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        
    }

    // Checks if the player is grounded by casting a box beneath the player's collider
    public bool isGrounded()
    {
        // Perform a box cast slightly below the player's collider
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,  // Start from the collider's center
            boxCollider.bounds.size,   // Use the collider's size
            0,                         // No rotation
            Vector2.down,              // Direction of the cast
            0.1f,                      // Distance to check
            groundLayer                // Layer to check against
        );

        // Return true if the box cast hits a ground collider
        return raycastHit.collider != null;
    }

    // Provides external access to the horizontal input value
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }

}