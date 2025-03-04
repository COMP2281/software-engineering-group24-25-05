using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float maxSlopeAngle = 45f;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 targetVelocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);

        // Raycast down to detect slope
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle <= maxSlopeAngle)
            {
                // Project movement along slope
                Vector2 slopeDirection = Vector2.Perpendicular(hit.normal) * Mathf.Sign(movement.x);
                targetVelocity.x = slopeDirection.x * moveSpeed;
            }
        }

        // Apply acceleration
        float newX = Mathf.Lerp(rb.velocity.x, targetVelocity.x, acceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector2(newX, rb.velocity.y);
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
