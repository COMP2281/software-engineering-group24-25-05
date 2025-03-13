using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints; // Patrol waypoints
    public float speed = 2f; // Enemy moving speed
    private int currentWaypointIndex = 0; 
    private int previousWaypointIndex = 0; 
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer

    // Vision parameters
    public float viewRange = 10f; // Vision range (tbc)
    public float viewAngle = 60f; // Vision angle (tbc could adjust if needed)
    public Transform player; 
    public float eyeHeight = 0.5f; // Eye height offset
    public Vector2 raycastOffset = Vector2.zero; // Raycast offset

    // UI part (still needs to be completed)
    public GameObject questionUI; // Answering UI

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component for movement
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position; // Set the initial position to the first waypoint
    }

    void Update()
    {
        Patrol(); // Patrol the waypoints
        CheckForPlayer(); // Check if the player is in the enemy's vision range
    }

    // Patrol behavior
    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 directionToWaypoint = (targetWaypoint.position - transform.position).normalized; 

        // Set sprite direction based on movement
        if (directionToWaypoint.x > 0) {
            spriteRenderer.flipX = true; // Moving right, don't flip
        } else if (directionToWaypoint.x < 0) {
            spriteRenderer.flipX = false; // Moving left, flip sprite
        }

        // i set enemy could move horizontally only, keep vertical speed unchanged.
        rb.velocity = new Vector2(directionToWaypoint.x * speed, rb.velocity.y); 

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            previousWaypointIndex = currentWaypointIndex; 
            currentWaypointIndex++; // Move to the next waypoint
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0; // Loop back to the first waypoint
        }
    }

    // Check if the player is within the enemy's vision range
    void CheckForPlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is not set in the inspector!");
            return;
        }
        
        // Determine facing direction based on sprite orientation
        Vector2 facingDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
        
        // Setup raycast origin at eye height
        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(0, eyeHeight);
        
        // Get all colliders hit by the ray
        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, facingDirection, viewRange);
        
        // Draw the actual raycast being used
        Debug.DrawRay(raycastOrigin, facingDirection * viewRange, Color.green);
        
        // Filter out self-collisions
        var validHits = hits.Where(hit => hit.collider.gameObject != gameObject).OrderBy(hit => hit.distance).ToArray();
        
        if (validHits.Length > 0)
        {
            var firstHit = validHits[0]; // Get the closest non-self hit
            Debug.Log($"Hit object: {firstHit.collider.gameObject.name}, Tag: {firstHit.collider.tag}, Distance: {firstHit.distance}");
            
            if (firstHit.collider.CompareTag("Player"))
            {
                rb.velocity = Vector2.zero; // Stop the enemy's movement
                Debug.Log("Player detected, triggering question!");
                TriggerQuestionUI(); // Trigger question UI
            }
        }
    }

    void TriggerQuestionUI()
    {
        if (questionUI != null)
        {
            questionUI.SetActive(true); // Display the UI
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {            rb.velocity = Vector2.zero; // Stop the enemy's movement
        TriggerQuestionUI();
        }
    }   
    void UpdateViewDirection()
    {
        Vector3 moveDirection = rb.velocity.normalized;
        if (moveDirection.magnitude > 0)
        {
            transform.up = moveDirection;
            }
        }
    void OnDrawGizmos()
    {
        if (spriteRenderer == null) return;
        
        // Draw view range radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);
        
        // Determine facing direction based on sprite orientation
        Vector2 facingDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
        
        // Calculate eye position
        Vector3 eyePosition = transform.position + new Vector3(0, eyeHeight, 0);
        
        // Draw view cone from eye position
        Vector3 rightBoundary = Quaternion.Euler(0, 0, viewAngle / 2) * facingDirection;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -viewAngle / 2) * facingDirection;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(eyePosition, rightBoundary * viewRange);
        Gizmos.DrawRay(eyePosition, leftBoundary * viewRange);
        
        // Mark eye position and draw straight vision ray
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(eyePosition, 0.1f);
        Gizmos.DrawRay(eyePosition, facingDirection * viewRange);
    }
}
