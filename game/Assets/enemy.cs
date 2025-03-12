using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints; 
    public float speed = 2f; // Enemy moving speed
    private int currentWaypointIndex = 0; // Patrol index

    private Rigidbody2D rb;
    private Vector2 movementDirection;

    // Vision parameters
    public float viewRange = 20f; 
    public float viewAngle = 60f; 
    public LayerMask viewMask; 
    private bool isChasing = false; // Whether the enemy is chasing the player
    public Transform player; 

    // UI part (still needs to be completed)
    public GameObject questionUI; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position; //initial position of the first waypoint
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer(); 
        }
        else
        {
            Patrol(); 
            CheckForPlayer(); 
        }

        UpdateViewDirection(); 
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        movementDirection = (targetWaypoint.position - transform.position).normalized; 

        rb.linearVelocity = movementDirection * speed; // Move towards the next waypoint

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0; // Loop back
        }
    }

    void CheckForPlayer()
    {
        Vector3 dirToPlayer = player.position - transform.position;

        float angleToPlayer = Vector3.Angle(transform.up, dirToPlayer); 

        Debug.Log("Angle to player: " + angleToPlayer);
        Debug.Log("Distance to player: " + dirToPlayer.magnitude);

        if (dirToPlayer.magnitude < viewRange && angleToPlayer < viewAngle / 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer.normalized, viewRange, viewMask);

            if (hit.collider != null)
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
            }

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                isChasing = true; 
                Debug.Log("Chasing player...");
            }
        }
    }

    void ChasePlayer()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized; 
        rb.linearVelocity = dirToPlayer * speed; // Move toward player

        Debug.Log("Chasing player...");

        // check if reached player
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
            questionUI.SetActive(true); // Display the UI then
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero; // Stop the enemy's movement
            Debug.Log("Caught the player!");
            TriggerQuestionUI(); // Trigger the answering UI
        }
    }

    void UpdateViewDirection()
    {
        Vector3 moveDirection = rb.linearVelocity.normalized;

        if (moveDirection.magnitude > 0)
        {
            transform.up = moveDirection; 
        }
    }
}
