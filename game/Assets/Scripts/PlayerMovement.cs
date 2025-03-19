using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 11f;
    [SerializeField] private float doubleJumpForce = 10f; // Slightly smaller force for double jump
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private DialogueManager dialogueManager;

    // Viewcone reference
    [SerializeField] private PlayerViewcone playerViewcone;

    // Simplified jump parameters
    [SerializeField] private float fallGravityScale = 2.2f;
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float fastFallSpeed = 12f;
    [SerializeField] private float jumpCooldown = 0.1f;

    private Animator animator; // Animator reference

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump; // Track double jump availability
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float jumpCooldownCounter;
    private float defaultGravityScale;

    private bool isCrouching;
    private CapsuleCollider2D capsuleCollider;
    private Vector3 originalScale;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    [SerializeField] private float groundAngleThreshold = 0.7f; // Cosine of ~45 degrees
    [SerializeField] private LayerMask GroundLayer; // Layer for ground objects
    [SerializeField] private LayerMask CrouchCheckLayer; // Layers to check for obstacles when uncrouching
    private float horizontalVelocityBeforeLanding;

    // Player facing direction
    private bool isFacingLeft = false;
    private SpriteRenderer spriteRenderer;

    // Track previous dialogue state for change detection
    private bool wasDialogueActiveLastFrame = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalColliderSize = capsuleCollider.size;
        originalColliderOffset = capsuleCollider.offset;
        defaultGravityScale = rb.gravityScale;

        // Get viewcone component if not assigned
        if (playerViewcone == null)
            playerViewcone = GetComponent<PlayerViewcone>();

        // Subscribe to direction change events
        if (playerViewcone != null)
            playerViewcone.OnDirectionChanged += OnPlayerDirectionChanged;
    }

    void Start()
    {

    }

    void Update()
    {

        // Check if dialogue state has changed
        bool isDialogueActive = dialogueManager.dialogueBox.activeSelf;
        if (isDialogueActive != wasDialogueActiveLastFrame)
        {
            wasDialogueActiveLastFrame = isDialogueActive;

            // Toggle viewcone based on dialogue state
            if (playerViewcone != null)
            {
                playerViewcone.SetViewconeActive(!isDialogueActive);
            }
        }

        // Block all input if dialogue is open
        if (isDialogueActive)
            return;

        // Manage jump cooldown
        if (jumpCooldownCounter > 0)
        {
            jumpCooldownCounter -= Time.deltaTime;
        }

        // Manage coyote time and reset double jump when grounded
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            canDoubleJump = true; // Reset double jump when on ground
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Manage jump buffer
        if (UserInput.Instance.JumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // First jump - when grounded or in coyote time
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isCrouching && jumpCooldownCounter <= 0f)
        {
            PerformJump(jumpForce);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
        // Double jump - when already in air and double jump is available
        else if (jumpBufferCounter > 0f && !isGrounded && canDoubleJump && jumpCooldownCounter <= 0f)
        {
            PerformJump(doubleJumpForce);
            canDoubleJump = false; // Use up the double jump
            jumpBufferCounter = 0f;
        }

        // Crouch handling
        if (UserInput.Instance.CrouchHold)
        {
            if (!isCrouching)
                StartCrouch();
        }
        else
        {
            if (isCrouching)
                StopCrouch();
        }

        // Apply fall gravity
        ApplyJumpPhysics();
        // Handling linking animations to movements
        HandleAnimations();
    }

    // Helper method to perform jump with given force
    private void PerformJump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Clear existing Y velocity
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        jumpCooldownCounter = jumpCooldown;
    }

    private void ApplyJumpPhysics()
    {
        // Falling - apply enhanced gravity but cap maximum fall speed
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravityScale;

            // Cap fall speed
            if (rb.velocity.y < -fastFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -fastFallSpeed);
            }
        }
        // Reset to default for rising
        else
        {
            rb.gravityScale = defaultGravityScale;
        }
    }

    void FixedUpdate()
    {
        // Skip movement if dialogue is open
        QuestionUI globalUI = FindObjectOfType<QuestionUI>();
        if (dialogueManager.dialogueBox.activeSelf || globalUI.IsVisible())
            return;

        Vector2 movement = UserInput.Instance.MovementInput;

        // Remove the control inversion - deleted the code that inverted controls

        float effectiveSpeed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed;
        Vector2 targetVelocity = new Vector2(movement.x * effectiveSpeed, rb.velocity.y);

        // Previous code which momentum led to delayed animation exiting
        // float newX = Mathf.Lerp(rb.velocity.x, targetVelocity.x, acceleration * Time.fixedDeltaTime);
        // rb.velocity = new Vector2(newX, rb.velocity.y);

        if (Mathf.Abs(movement.x) > 0.1f)
        {
            // Apply normal movement with  acceleration
            float newX = Mathf.Lerp(rb.velocity.x, targetVelocity.x, acceleration * Time.fixedDeltaTime);
            rb.velocity = new Vector2(newX, rb.velocity.y);
        }
        else if (isGrounded)
        {
            // Stop movement instantly when grounded and no input
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // Store horizontal velocity before landing
        if (!isGrounded)
        {
            horizontalVelocityBeforeLanding = rb.velocity.x;
        }
    }

    private void HandleAnimations()
    {
        // Get absolute horizontal velocity
        float moveInput = Mathf.Abs(rb.velocity.x);
        // Updating Speed parameter
        animator.SetFloat("Speed", moveInput);

        // Jump animation
        if (!isGrounded)
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
        if (moveInput < 0.1f && isGrounded)
        {
            // Ensures animation transitions immediately
            animator.SetFloat("Speed", 0f);
            // Forces Idle animation if Run lingers
            animator.Play("Idle"); // Changed from "Player" to "Idle"
        }
    }

    void StartCrouch()
    {
        isCrouching = true;
        // Calculate offset using original scale to keep bottom fixed.
        float yOffset = (originalColliderSize.y * originalScale.y - originalColliderSize.y * originalScale.y * 0.5f) / 2;
        transform.position -= new Vector3(0, yOffset, 0);
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        capsuleCollider.size = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f);
        capsuleCollider.offset = originalColliderOffset;
    }

    void StopCrouch()
    {
        // Check if we can safely uncrouch
        if (!CanUncrouch())
        {
            // Can't uncrouch yet, stay crouched
            return;
        }

        isCrouching = false;
        float yOffset = (originalColliderSize.y * originalScale.y - originalColliderSize.y * originalScale.y * 0.5f) / 2;
        transform.position += new Vector3(0, yOffset, 0);
        transform.localScale = originalScale;
        capsuleCollider.size = originalColliderSize;
        capsuleCollider.offset = originalColliderOffset;
    }

    // Check if it's safe to uncrouch by looking for obstacles above
    private bool CanUncrouch()
    {
        // Calculate the position and size of the box check
        Vector2 boxCenter = transform.position + new Vector3(0, originalColliderSize.y * 0.75f, 0);
        Vector2 boxSize = new Vector2(originalColliderSize.x * 0.9f, originalColliderSize.y * 0.5f);

        // Check for obstacles above the player
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, CrouchCheckLayer);

        // Filter out the player's own collider
        foreach (Collider2D collider in colliders)
        {
            if (collider != capsuleCollider && collider.gameObject != gameObject)
            {
                // Found an obstacle, can't uncrouch
                return false;
            }
        }

        // No obstacles found, can uncrouch
        return true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is with a ground layer object
        if (((1 << collision.gameObject.layer) & GroundLayer) != 0)
        {
            // Check if this is a ground collision by examining contact normals
            CheckGroundContact(collision);

            // Reapply horizontal velocity to maintain momentum (only if truly grounded)
            if (isGrounded)
            {
                rb.velocity = new Vector2(horizontalVelocityBeforeLanding, rb.velocity.y);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Check if collision is with a ground layer object
        if (((1 << collision.gameObject.layer) & GroundLayer) != 0)
        {
            // Continuously check ground contact while colliding
            CheckGroundContact(collision);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if collision is with a ground layer object
        if (((1 << collision.gameObject.layer) & GroundLayer) != 0)
        {
            isGrounded = false;
        }
    }

    private void CheckGroundContact(Collision2D collision)
    {
        isGrounded = false;

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;

            // If normal.y is greater than our threshold, this is ground
            if (normal.y >= groundAngleThreshold)
            {
                isGrounded = true;
                break;
            }
            // We don't need to track wall contact since we're not using it
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe when destroyed to prevent memory leaks
        if (playerViewcone != null)
            playerViewcone.OnDirectionChanged -= OnPlayerDirectionChanged;
    }

    // Called when the mouse changes sides
    private void OnPlayerDirectionChanged(bool isMouseOnLeft)
    {
        // Update facing direction
        isFacingLeft = isMouseOnLeft;

        // Flip the sprite accordingly
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = isFacingLeft;
        }
        else
        {
            // If no SpriteRenderer, flip the transform scale instead
            Vector3 currentScale = transform.localScale;
            currentScale.x = isFacingLeft ? -Mathf.Abs(originalScale.x) : Mathf.Abs(originalScale.x);
            transform.localScale = currentScale;
        }

        // Make sure the light position is updated immediately
        if (playerViewcone != null)
        {
            playerViewcone.RefreshLightPosition();
        }
    }

    public Rigidbody2D GetRigidBody()
    {
        return this.rb;
    }
}
