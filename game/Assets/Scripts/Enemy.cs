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
    public float eyeHeight = 0.5f; // Eye height offset for raycast
    public float viewconeHeight = 0f; // Vertical position for viewcone light (adjust in inspector)
    public float eyeOffset = 0.5f; // Horizontal offset for the eye position
    public Transform viewconeLight; // Reference to the viewcone light transform (if any)

    // UI part (still needs to be completed)
    public QuestionUI questionUI; // Answering UI

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component for movement
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position; // Set the initial position to the first waypoint

        // Hide the UI initially
        questionUI.MakeVisible(false);
    }

    void Update()
    {
        Patrol(); // Patrol the waypoints
        CheckForPlayer(); // Check if the player is in the enemy's vision range
        UpdateViewDirection(); // Update the view direction based on movement
    }

    // Patrol behavior
    void Patrol()
    {
        if (waypoints.Length == 0) return;

        // Do not patrol if we are currently asking a question
        if (this.questionUI.IsVisible()) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 directionToWaypoint = (targetWaypoint.position - transform.position).normalized;

        // IMPORTANT: Fixed sprite flipping logic to be consistent
        if (directionToWaypoint.x > 0)
        {
            spriteRenderer.flipX = true; // Moving right - sprite faces right
        }
        else if (directionToWaypoint.x < 0)
        {
            spriteRenderer.flipX = false; // Moving left - sprite faces left
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

        // Get facing direction based on sprite orientation
        Vector2 facingDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;

        // Determine horizontal offset sign based on facing direction
        float offsetSign = facingDirection.x;

        // Setup raycast origin at eye height and with horizontal offset based on facing direction
        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(offsetSign * eyeOffset, eyeHeight);

        // Get all colliders hit by the ray
        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, facingDirection, viewRange);

        // Draw the actual raycast being used
        Debug.DrawRay(raycastOrigin, facingDirection * viewRange, Color.green);

        // Filter out self-collisions
        var validHits = hits.Where(hit => hit.collider.gameObject != gameObject).OrderBy(hit => hit.distance).ToArray();

        // Check all hits in order of distance
        foreach (var hit in validHits)
        {
            // If we hit a cover object first, the player cannot be seen
            if (hit.collider.CompareTag("Cover"))
            {
                // Debug.Log("View blocked by cover");
                return; // Cover blocks the view - exit without detecting player
            }

            // If we hit the player before any cover, detect them
            if (hit.collider.CompareTag("Player"))
            {
                rb.velocity = Vector2.zero; // Stop the enemy's movement

                // Set enemy speed to zero when player is detected
                rb.velocity = Vector2.zero; // Stop the enemy's movements
                TriggerQuestionUI(); // Trigger question UI
                return;
            }
        }
    }

    void TriggerQuestionUI()
    {
        if (this.questionUI.GetCanQuestion())
        {
            Debug.Log("Player detected, triggering question!");

            this.questionUI.SetCanQuestion(false);

            this.questionUI.SetCorrectAnswerCallback(() =>
            {
                Debug.Log("Correct Answer!");
                this.questionUI.SetTimeSinceQuestion(0);

                // TODO: What to do if answer is correct?
            });

            this.questionUI.SetIncorrectAnswerCallback(() =>
            {
                Debug.Log("Incorrect Answer!");
                this.questionUI.SetTimeSinceQuestion(0);

                // TODO: What to do if answer is incorrect?
            });

            this.questionUI.ResetAll();
            this.questionUI.MakeVisible(true);
            this.questionUI.LoadNextQuestion();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero; // Stop the enemy's movement
            TriggerQuestionUI();
        }
    }
    void UpdateViewDirection()
    {
        // Get facing direction based on sprite orientation
        Vector2 facingDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;

        // Rotate viewcone light if it exists
        if (viewconeLight != null)
        {
            // Position the viewcone at the eye position with custom height
            viewconeLight.localPosition = new Vector3(facingDirection.x * eyeOffset, viewconeHeight, 0);

            // Use scale to flip
            viewconeLight.localScale = new Vector3(spriteRenderer.flipX ? 1 : -1, 1, 1);

            // Adjust rotation to make viewcone face horizontally
            if (spriteRenderer.flipX)
            {
                // Facing right
                viewconeLight.localRotation = Quaternion.Euler(0, 0, -90); // Rotate to face right horizontally
            }
            else
            {
                // Facing left
                viewconeLight.localRotation = Quaternion.Euler(0, 0, 90); // Rotate to face left horizontally
            }
        }
    }

    void OnDrawGizmos()
    {
        if (spriteRenderer == null) return;

        // Draw view range radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        // Get facing direction based on sprite orientation
        Vector2 facingDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;

        // Calculate eye position with both height and horizontal offset
        Vector3 eyePosition = transform.position + new Vector3(facingDirection.x * eyeOffset, eyeHeight, 0);

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

        // Also visualize viewcone light position if applicable
        if (viewconeLight != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position + new Vector3(facingDirection.x * eyeOffset, viewconeHeight, 0), 0.08f);
        }
    }
}
