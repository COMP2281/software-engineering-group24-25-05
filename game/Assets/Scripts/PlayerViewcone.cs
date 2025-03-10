using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // For 2D lights in URP

public class PlayerViewcone : MonoBehaviour
{
    [SerializeField] private Light2D viewconeLight;
    [SerializeField] private float viewconeAngle = 60f;
    [SerializeField] private float viewconeDistance = 10f;
    [SerializeField] private float innerConePercentage = 0.7f;
    [SerializeField] private float intensityInCone = 1.0f;
    [SerializeField] private Camera mainCamera;
    
    // Optional references for visual feedback
    [SerializeField] private Transform viewconeDirection;
    [SerializeField] private Color viewconeColor = new Color(1f, 1f, 0.8f, 1f);
    
    // Angle correction to align with mouse
    [SerializeField] private float angleCorrection = -90f;

    // Public property to track if mouse is on the left side
    public bool IsMouseOnLeft { get; private set; }
    
    // Event for when player direction changes
    public System.Action<bool> OnDirectionChanged;

    // Light positioning with adjusted values for lower position
    [SerializeField] private Vector2 lightOffset = new Vector2(0.5f, -0.3f); // Using negative Y to position below center
    
    // Light positioning - track parameters
    [SerializeField] private float trackRadius = 0.5f; // Distance from center for the track
    [SerializeField] private float trackYOffset = 0f; // Y offset for the track center (changed from -0.3f to 0f)
    [SerializeField] private float rotationLimitDegrees = 75f; // Limit how far the light can rotate up/down
    [SerializeField] private float minimumLightHeight = -0.2f; // Minimum height for the light
    private Vector3 directionToMouse;

    // Track if viewcone is active
    private bool isViewconeActive = true;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        if (viewconeLight == null)
        {
            // Create light if not assigned in inspector
            GameObject lightObj = new GameObject("ViewconeLight");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = new Vector3(lightOffset.x, lightOffset.y, 0);
            
            viewconeLight = lightObj.AddComponent<Light2D>();
            viewconeLight.lightType = Light2D.LightType.Point;
            viewconeLight.color = viewconeColor;
        }
        
        // Configure the light properties
        ConfigureLight();
    }
    
    private void ConfigureLight()
    {
        if (viewconeLight != null)
        {
            viewconeLight.pointLightInnerAngle = viewconeAngle * innerConePercentage;
            viewconeLight.pointLightOuterAngle = viewconeAngle;
            viewconeLight.pointLightOuterRadius = viewconeDistance;
            viewconeLight.intensity = intensityInCone;
            viewconeLight.shadowIntensity = 1f;
            
            // Make sure the light type is set to Point
            viewconeLight.lightType = Light2D.LightType.Point;
        }
    }

    private void Update()
    {
        if (viewconeLight == null || !isViewconeActive) return;
        
        // Get mouse position in world space
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;  // Ensure we're on the same Z plane as the player
        
        // Calculate direction from player to mouse
        directionToMouse = mouseWorldPos - transform.position;
        
        // Check if mouse is on the left side of player
        bool newIsMouseOnLeft = directionToMouse.x < 0;
        
        // If direction changed, notify listeners
        if (newIsMouseOnLeft != IsMouseOnLeft) {
            IsMouseOnLeft = newIsMouseOnLeft;
            OnDirectionChanged?.Invoke(IsMouseOnLeft);
        }
        
        // Calculate angle to point the light
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        
        // Apply angle correction to align with mouse
        float correctedAngle = angle + angleCorrection;
        
        // Update light position based on angle (track system)
        UpdateLightPositionOnTrack(angle);
        
        // Set light rotation
        viewconeLight.transform.rotation = Quaternion.Euler(0, 0, correctedAngle);
        
        // Update optional visual direction indicator if assigned
        if (viewconeDirection != null)
        {
            viewconeDirection.rotation = Quaternion.Euler(0, 0, correctedAngle);
        }
        
        // Debug visual line to see the direction
        Debug.DrawRay(transform.position, directionToMouse.normalized * 5f, Color.red);
    }
    
    // Update light position to follow a circular track
    private void UpdateLightPositionOnTrack(float angle)
    {
        if (viewconeLight != null)
        {
            // Limit the angle for vertical movement
            float clampedAngle = angle;
            
            // Clamp angle differently based on facing direction
            if (IsMouseOnLeft) {
                // When facing left, limit to ±rotationLimitDegrees from horizontal left (-180 degrees)
                float baseLeftAngle = -180f;
                if (clampedAngle > 0) clampedAngle = clampedAngle - 360; // Convert to negative range
                clampedAngle = Mathf.Clamp(clampedAngle, baseLeftAngle - rotationLimitDegrees, baseLeftAngle + rotationLimitDegrees);
            } else {
                // When facing right, limit to ±rotationLimitDegrees from horizontal right (0 degrees)
                clampedAngle = Mathf.Clamp(clampedAngle, -rotationLimitDegrees, rotationLimitDegrees);
            }
            
            // Convert angle to radians for position calculation
            float radians = clampedAngle * Mathf.Deg2Rad;
            
            // Calculate position on the track
            float x = Mathf.Cos(radians) * trackRadius;
            float y = Mathf.Sin(radians) * trackRadius + trackYOffset;
            
            // Ensure the light doesn't go below the minimum height
            y = Mathf.Max(y, minimumLightHeight);
            
            // Apply the new position
            viewconeLight.transform.localPosition = new Vector3(x, y, 0);
        }
    }
    
    // Public method to manually update light position (can be called from outside)
    public void RefreshLightPosition()
    {
        if (viewconeLight != null && directionToMouse != Vector3.zero) {
            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            UpdateLightPositionOnTrack(angle);
        }
    }
    
    // Optional method to fine-tune the light position in game
    public void AdjustLightHeight(float heightAdjustment)
    {
        lightOffset.y += heightAdjustment;
        UpdateLightPositionOnTrack(Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg);
    }

    // Method to toggle the viewcone light
    public void SetViewconeActive(bool active)
    {
        isViewconeActive = active;
        
        if (viewconeLight != null)
        {
            viewconeLight.enabled = active;
        }
    }
}
