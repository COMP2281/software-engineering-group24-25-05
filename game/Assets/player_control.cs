using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // player moving speed
    private Rigidbody2D rb; 
    private Vector2 movement; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        movement = new Vector2(moveX, moveY).normalized; 
    }

    void FixedUpdate()
    {

        rb.linearVelocity = movement * moveSpeed; 
    }
}
