using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Add this line to import the PlayerInput class

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = playerInput.actions["Move"].ReadValue<Vector2>();
        transform.Translate(new Vector3(movement.x, 0f, movement.y) * moveSpeed * Time.deltaTime);
    }
}
