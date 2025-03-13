using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints; // Patrol waypoints
    public float speed = 2f; // Enemy moving speed
    public float chaseSpeed = 4f; // Enemy chase speed
    private int currentWaypointIndex = 0; 
    private bool isChasing = false; 
    private Rigidbody2D rb;

    // Vision parameters
    public float viewRange = 10f; // Vision range
    public float viewAngle = 60f; // Vision angle
    public LayerMask viewMask; 
    public Transform player; 
    public float stopChaseDistance = 2f; // Distance to stop chasing when the player moves away

    // UI part (still needs to be completed)
    public GameObject questionUI; // Answering UI

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component for movement
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position; // Set the initial position to the first waypoint
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer(); // Start chasing the player
            if (Vector2.Distance(transform.position, player.position) > stopChaseDistance)
            {
                isChasing = false; // Stop chasing
                Debug.Log("Player escaped. Returning to patrol.");
            }
        }
        else
        {
            Patrol(); // Patrol the waypoints
            CheckForPlayer(); // Check if the player is in the enemy's vision range
        }

        UpdateViewDirection(); // Update the enemy's vision direction to follow the movement direction
    }

    // Patrol behavior
    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 directionToWaypoint = (targetWaypoint.position - transform.position).normalized; 

        // Set enemy to move horizontally only, keep vertical speed unchanged.
        rb.linearVelocity = new Vector2(directionToWaypoint.x * speed, rb.linearVelocity.y); 

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex++; // Move to the next waypoint
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0; // Loop back to the first waypoint
        }
    }

    // Check if the player is within the enemy's vision range
    void CheckForPlayer()
    {
        Vector3 dirToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.up, dirToPlayer); 

        if (dirToPlayer.magnitude < viewRange && angleToPlayer < viewAngle / 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer.normalized, viewRange, viewMask);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                isChasing = true; // Start chasing the player
                Debug.Log("Chasing player...");
            }
        }
    }

    // Chasing behavior when the enemy starts following the player
    void ChasePlayer()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized; 
        rb.linearVelocity = new Vector2(dirToPlayer.x * chaseSpeed, rb.linearVelocity.y); // Move towards the player along the x-axis (horizontal only)

        if (Vector2.Distance(transform.position, player.position) < 1.5f)
        {
            TriggerQuestionUI(); // Trigger the answering UI
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
        {
            rb.linearVelocity = Vector2.zero; // Stop the enemy's movement
            TriggerQuestionUI(); 
        }
    }

    // Update the enemy's vision direction to follow the movement direction
    void UpdateViewDirection()
    {
        
        if (rb.linearVelocity.x > 0)
        {
            transform.up = Vector2.right; // Vision direction to the right
        }
        else if (rb.linearVelocity.x < 0)
        {
            transform.up = Vector2.left; // Vision direction to the left
        }
    }
}
