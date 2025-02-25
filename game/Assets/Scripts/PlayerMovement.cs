using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 6.5f; // Just enough to allow to jump 2 grid high
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float jumpHoldForce = 4f; // increased from 3f
    [SerializeField] private float jumpHoldDuration = 0.3f; // increased from 0.2f
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;

    private bool isCrouching;
    private CapsuleCollider2D capsuleCollider;
    private Vector3 originalScale;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        originalScale = transform.localScale;
        originalColliderSize = capsuleCollider.size;
        originalColliderOffset = capsuleCollider.offset;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (playerInput.actions["Jump"].triggered && isGrounded && !isCrouching)
        {
            isJumping = true;
            jumpTimeCounter = jumpHoldDuration;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (playerInput.actions["Jump"].IsPressed() && isJumping)
        {
            if (jumpTimeCounter > 0 && rb.velocity.y > 0) // Only apply force while moving up
            {
                rb.AddForce(Vector2.up * jumpHoldForce, ForceMode2D.Force);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (playerInput.actions["Jump"].WasReleasedThisFrame())
        {
            isJumping = false;
        }

        if (playerInput.actions["Crouch"].IsPressed())
        {
            if (!isCrouching)
                StartCrouch();
        }
        else
        {
            if (isCrouching)
                StopCrouch();
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = playerInput.actions["Move"].ReadValue<Vector2>();
        float effectiveSpeed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed;
        Vector2 targetVelocity = new Vector2(movement.x * effectiveSpeed, rb.velocity.y);

        float newX = Mathf.Lerp(rb.velocity.x, targetVelocity.x, acceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector2(newX, rb.velocity.y);

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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
