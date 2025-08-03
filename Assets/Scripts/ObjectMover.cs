using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;
    public Vector3 direction = Vector3.down;
    
    [Header("Rotation")]
    public bool rotateObject = false;
    public float rotationSpeed = 90f;
    
    void Update()
    {
        // Move object
        transform.Translate(direction * speed * Time.deltaTime);
        
        // Rotate object if enabled
        if (rotateObject)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
} 