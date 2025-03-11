using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints; // patrol waypoint
    public float speed = 2f; // enemy moving speed
    private int currentWaypointIndex = 0; // patrol index

    private Rigidbody2D rb;
    private Vector2 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position; // set the start position
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        movementDirection = (targetWaypoint.position - transform.position).normalized;

        rb.linearVelocity = movementDirection * speed;

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0;
        }
    }

    // triggered questions
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero; // stop
            Debug.Log("triggered！");
            // call questionc UI then
        }
    }
}

