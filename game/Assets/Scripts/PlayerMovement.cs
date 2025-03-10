using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 11f;
    [SerializeField] private float doubleJumpForce = 10f; // Slightly smaller force for double jump
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private DialogueManager dialogueManager;
    
    // Simplified jump parameters
    [SerializeField] private float fallGravityScale = 2.2f;  
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float fastFallSpeed = 12f;
    [SerializeField] private float jumpCooldown = 0.1f;
    
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
    private float horizontalVelocityBeforeLanding;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        originalScale = transform.localScale;
        originalColliderSize = capsuleCollider.size;
        originalColliderOffset = capsuleCollider.offset;
        defaultGravityScale = rb.gravityScale;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Block all input if dialogue is open
        if (dialogueManager.dialogueBox.activeSelf)
            return;

        // Manage jump cooldown
        if (jumpCooldownCounter > 0) {
            jumpCooldownCounter -= Time.deltaTime;
        }

        // Manage coyote time and reset double jump when grounded
        if (isGrounded) {
            coyoteTimeCounter = coyoteTime;
            canDoubleJump = true; // Reset double jump when on ground
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Manage jump buffer
        if (UserInput.Instance.JumpPressed) {
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }

        // First jump - when grounded or in coyote time
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isCrouching && jumpCooldownCounter <= 0f) {
            PerformJump(jumpForce);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
        // Double jump - when already in air and double jump is available
        else if (jumpBufferCounter > 0f && !isGrounded && canDoubleJump && jumpCooldownCounter <= 0f) {
            PerformJump(doubleJumpForce);
            canDoubleJump = false; // Use up the double jump
            jumpBufferCounter = 0f;
        }

        // Crouch handling
        if (UserInput.Instance.CrouchHold) {
            if (!isCrouching)
                StartCrouch();
        }
        else {
            if (isCrouching)
                StopCrouch();
        }
        
        // Apply fall gravity
        ApplyJumpPhysics();
    }

    // Helper method to perform jump with given force
    private void PerformJump(float force) {
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Clear existing Y velocity
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        jumpCooldownCounter = jumpCooldown;
    }

    private void ApplyJumpPhysics() {
        // Falling - apply enhanced gravity but cap maximum fall speed
        if (rb.velocity.y < 0) {
            rb.gravityScale = fallGravityScale;
            
            // Cap fall speed
            if (rb.velocity.y < -fastFallSpeed) {
                rb.velocity = new Vector2(rb.velocity.x, -fastFallSpeed);
            }
        }
        // Reset to default for rising
        else {
            rb.gravityScale = defaultGravityScale;
        }
    }

    void FixedUpdate()
    {
        // Skip movement if dialogue is open
        if (dialogueManager.dialogueBox.activeSelf)
            return;

        Vector2 movement = UserInput.Instance.MovementInput;
        float effectiveSpeed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed;
        Vector2 targetVelocity = new Vector2(movement.x * effectiveSpeed, rb.velocity.y);

        float newX = Mathf.Lerp(rb.velocity.x, targetVelocity.x, acceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector2(newX, rb.velocity.y);

        // Store horizontal velocity before landing
        if (!isGrounded)
        {
            horizontalVelocityBeforeLanding = rb.velocity.x;
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
        isCrouching = false;
        float yOffset = (originalColliderSize.y * originalScale.y - originalColliderSize.y * originalScale.y * 0.5f) / 2;
        transform.position += new Vector3(0, yOffset, 0);
        transform.localScale = originalScale;
        capsuleCollider.size = originalColliderSize;
        capsuleCollider.offset = originalColliderOffset;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is with a ground layer object
        if (((1 << collision.gameObject.layer) & GroundLayer) != 0)
        {
            // Check if this is a ground collision by examining contact normals
            CheckGroundContact(collision);
            
            // Reapply horizontal velocity to maintain momentum (only if truly grounded)
            if (isGrounded) {
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
}
