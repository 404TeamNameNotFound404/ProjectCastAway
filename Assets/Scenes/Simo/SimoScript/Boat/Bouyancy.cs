using UnityEngine;

public class Bouyancy : MonoBehaviour
{
    public float buoyancyForce = 50f;
    public float dampingFactor = 0.5f;  // Add a damping factor to reduce overshooting
    public float surfaceLevel = 0f;     // Define the water surface level

    private Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ApplyBuoyancy();
    }

    void ApplyBuoyancy()
    {
        Vector3 position = rb.position;
        if (position.y < surfaceLevel)
        {
            // Calculate the distance below the surface level
            float distanceToSurface = surfaceLevel - position.y;

            // Apply buoyancy force proportional to the distance to the surface
            Vector3 force = new Vector3(0, buoyancyForce * distanceToSurface - dampingFactor * rb.linearVelocity.y, 0);
            rb.AddForce(force);
        }
    }
}
