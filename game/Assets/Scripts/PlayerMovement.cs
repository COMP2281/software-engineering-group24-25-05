using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float acceleration = 5f;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = playerInput.actions["Move"].ReadValue<Vector2>();

        float targetX = movement.x * moveSpeed;
        float newX = Mathf.Lerp(rb.velocity.x, targetX, acceleration * Time.deltaTime);

        rb.velocity = new Vector2(newX, rb.velocity.y);

        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Update animator parameters
        animator.SetFloat("Speed", Mathf.Abs(newX));
        animator.SetBool("IsJumping", !isGrounded);
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
