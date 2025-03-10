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

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        if (viewconeLight == null)
        {
            // Create light if not assigned in inspector
            GameObject lightObj = new GameObject("ViewconeLight");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = Vector3.zero;
            
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
        if (viewconeLight == null) return;
        
        // Get mouse position in world space
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;  // Ensure we're on the same Z plane as the player
        
        // Calculate direction from player to mouse
        Vector3 directionToMouse = mouseWorldPos - transform.position;
        
        // Calculate angle to point the light
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        
        // Apply angle correction to align with mouse
        float correctedAngle = angle + angleCorrection;
        
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
}
