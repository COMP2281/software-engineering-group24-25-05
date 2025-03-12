using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints; // Patrol waypoints
    public float speed = 2f; // Enemy moving speed
    private int currentWaypointIndex = 0; // Patrol index

    private Rigidbody2D rb;
    private Vector2 movementDirection;

    // Vision parameters
    public float viewRange = 20f; // Vision range (how far the enemy can see)
    public float viewAngle = 60f; // Vision angle (how wide the vision is)
    public LayerMask viewMask; // Layer mask to specify what the enemy can detect in its vision
    private bool isChasing = false; // Whether the enemy is chasing the player
    public Transform player; // Player object reference

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
        movementDirection = (targetWaypoint.position - transform.position).normalized; // Calculate direction to the next waypoint

        rb.linearVelocity = movementDirection * speed; // Move the enemy towards the next waypoint

        // Switch to the next waypoint when the enemy reaches the current one
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0; // Loop back to the first waypoint
        }
    }

    // Check if the player is within the enemy's vision range
    void CheckForPlayer()
    {
        // Calculate the direction to the player
        Vector3 dirToPlayer = player.position - transform.position;

        // Calculate the angle between the enemy's forward direction and the direction to the player
        float angleToPlayer = Vector3.Angle(transform.up, dirToPlayer); 

        // Debug: Print the angle and distance to the player for checking
        Debug.Log("Angle to player: " + angleToPlayer);
        Debug.Log("Distance to player: " + dirToPlayer.magnitude);

        // Check if the player is within the vision range and the angle to the player is within the vision angle
        if (dirToPlayer.magnitude < viewRange && angleToPlayer < viewAngle / 2)
        {
            // Use a raycast to check if there are obstacles between the enemy and the player
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer.normalized, viewRange, viewMask);

            // Debug: Print the raycast hit information for checking
            if (hit.collider != null)
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
            }

            // If the raycast hits the player, start chasing the player
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                isChasing = true; // Set chasing state to true
                Debug.Log("Chasing player...");
            }
        }
    }

    // Chasing behavior when the enemy starts following the player
    void ChasePlayer()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized; // Calculate direction to the player
        rb.linearVelocity = dirToPlayer * speed; // Move towards the player

        Debug.Log("Chasing player...");

        // Check if the enemy has reached the player (you can adjust the distance as needed)
        if (Vector2.Distance(transform.position, player.position) < 1.5f)
        {
            Debug.Log("Player reached! Triggering UI.");
            TriggerQuestionUI(); // Trigger the answering UI
        }
    }

    // Trigger the answering UI when the player is caught
    void TriggerQuestionUI()
    {
        if (questionUI != null)
        {
            questionUI.SetActive(true); // Display the UI
        }
    }

    // Trigger event when the enemy collides with the player
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero; // Stop the enemy's movement
            Debug.Log("Caught the player!");
            TriggerQuestionUI(); // Trigger the answering UI
        }
    }

    // Update the enemy's vision direction to follow the movement direction
    void UpdateViewDirection()
    {
        // Get the enemy's current movement direction
        Vector3 moveDirection = rb.linearVelocity.normalized;

        // If the enemy is moving, update its facing direction (vision direction)
        if (moveDirection.magnitude > 0)
        {
            transform.up = moveDirection; // Update the enemy's facing direction (vision direction)
        }
    }
}
