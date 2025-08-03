using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;        // El Player
    
    [Header("Follow Settings")]
    public float smoothSpeed = 5f;  // Suavidad del movimiento (mayor = más suave)
    public Vector3 offset = new Vector3(0, 0, -10); // Distancia de la cámara
    
    [Header("World Boundaries")]
    public float worldWidth = 20f;   // Ancho del mundo
    public float worldHeight = 15f;  // Alto del mundo
    
    [Header("Camera Settings")]
    public float cameraSize = 5f;    // Tamaño de la cámara ortográfica
    
    private Camera cam;
    private float halfWidth;
    private float halfHeight;
    
    void Start()
    {
        // Get camera component
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraFollow: No Camera component found!");
            return;
        }
        
        // Set camera to orthographic
        cam.orthographic = true;
        cam.orthographicSize = cameraSize;
        
        // Calculate boundaries
        halfWidth = worldWidth / 2f;
        halfHeight = worldHeight / 2f;
        
        // Set initial position
        if (target != null)
        {
            Vector3 initialPosition = target.position + offset;
            transform.position = ClampCameraPosition(initialPosition);
        }
        
        Debug.Log($"CameraFollow: Initialized - World: {worldWidth}x{worldHeight}, Camera Size: {cameraSize}");
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;
        
        // Clamp position to world boundaries
        Vector3 clampedPosition = ClampCameraPosition(desiredPosition);
        
        // Smoothly move camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
    
    /// <summary>
    /// Limita la posición de la cámara para que no salga del mundo
    /// </summary>
    Vector3 ClampCameraPosition(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, -halfWidth + cameraSize * 1.78f, halfWidth - cameraSize * 1.78f);
        float clampedY = Mathf.Clamp(position.y, -halfHeight + cameraSize, halfHeight - cameraSize);
        
        return new Vector3(clampedX, clampedY, position.z);
    }
    
    /// <summary>
    /// Dibuja los límites del mundo en el editor (solo para debug)
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(worldWidth, worldHeight, 0));
        
        // Draw camera bounds
        if (cam != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 cameraBounds = new Vector3(cameraSize * 1.78f * 2, cameraSize * 2, 0);
            Gizmos.DrawWireCube(transform.position, cameraBounds);
        }
    }
    
    // Public methods for external control
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    public void SetWorldSize(float width, float height)
    {
        worldWidth = width;
        worldHeight = height;
        halfWidth = worldWidth / 2f;
        halfHeight = worldHeight / 2f;
    }
    
    public void SetCameraSize(float size)
    {
        cameraSize = size;
        if (cam != null)
        {
            cam.orthographicSize = size;
        }
    }
    
    public void SetSmoothSpeed(float speed)
    {
        smoothSpeed = speed;
    }
} 