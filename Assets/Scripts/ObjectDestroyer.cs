using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [Header("Destroy Settings")]
    public float destroyHeight = -12f;
    public float destroyDelay = 0f;
    
    void Update()
    {
        // Check if object has passed the destroy height
        if (transform.position.y < destroyHeight)
        {
            DestroyObject();
        }
    }
    
    void DestroyObject()
    {
        if (destroyDelay > 0f)
        {
            Destroy(gameObject, destroyDelay);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetDestroyHeight(float height)
    {
        destroyHeight = height;
    }
    
    public void SetDestroyDelay(float delay)
    {
        destroyDelay = delay;
    }
} 