using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Header("Particle Systems")]
    public ParticleSystem fireParticles;
    public ParticleSystem sparkleParticles;
    
    [Header("Fire Particles Settings")]
    public Color fireColor = new Color(1f, 0.5f, 0f, 1f); // Orange
    public float fireEmissionRate = 12f; // Reduced from 20
    public float fireParticleLifetime = 0.6f; // Reduced from 1f
    public float fireSpeed = 1.5f; // Reduced from 2f
    public float fireParticleSize = 0.05f; // New: control particle size
    
    [Header("Sparkle Particles Settings")]
    public Color sparkleColor = new Color(0.5f, 0.8f, 1f, 1f); // Light blue
    public float sparkleEmissionRate = 8f; // Reduced from 15
    public float sparkleParticleLifetime = 0.5f; // Reduced from 0.8f
    public float sparkleSpeed = 1f; // Reduced from 1.5f
    public float sparkleParticleSize = 0.03f; // New: control particle size
    
    [Header("Positioning")]
    public Vector3 particleOffset = new Vector3(0f, -0.3f, 0f); // Below the sprite
    
    // Private variables
    private Transform playerTransform;
    private bool isInitialized = false;
    
    void Start()
    {
        Debug.Log("ParticleManager: Start() called");
        
        // Get player transform
        playerTransform = transform;
        
        // Initialize particle systems
        InitializeParticleSystems();
        
        isInitialized = true;
        Debug.Log("ParticleManager: Initialized successfully");
    }
    
    void Update()
    {
        if (isInitialized && playerTransform != null)
        {
            // Update particle positions to follow player
            UpdateParticlePositions();
        }
    }
    
    void InitializeParticleSystems()
    {
        // Create fire particles if not assigned
        if (fireParticles == null)
        {
            CreateFireParticles();
        }
        
        // Create sparkle particles if not assigned
        if (sparkleParticles == null)
        {
            CreateSparkleParticles();
        }
        
        // Start both particle systems
        if (fireParticles != null)
        {
            fireParticles.Play();
        }
        
        if (sparkleParticles != null)
        {
            sparkleParticles.Play();
        }
    }
    
    void CreateFireParticles()
    {
        // Create GameObject for fire particles
        GameObject fireGO = new GameObject("FireParticles");
        fireGO.transform.SetParent(transform);
        fireGO.transform.localPosition = particleOffset;
        
        // Add ParticleSystem component
        fireParticles = fireGO.AddComponent<ParticleSystem>();
        
        // Configure fire particles
        var main = fireParticles.main;
        main.startLifetime = fireParticleLifetime;
        main.startSpeed = fireSpeed;
        main.startColor = fireColor;
        main.startSize = fireParticleSize; // Set particle size
        main.maxParticles = 30; // Reduced from 50
        
        var emission = fireParticles.emission;
        emission.rateOverTime = fireEmissionRate;
        
        var shape = fireParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.05f; // Reduced from 0.1f
        
        var colorOverLifetime = fireParticles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(fireColor, 0.0f), 
                new GradientColorKey(fireColor * 0.8f, 1.0f) // Use fireColor instead of hardcoded red
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1.0f, 0.0f), 
                new GradientAlphaKey(0.0f, 1.0f) 
            }
        );
        colorOverLifetime.color = gradient;
        
        Debug.Log("ParticleManager: Fire particles created");
    }
    
    void CreateSparkleParticles()
    {
        // Create GameObject for sparkle particles
        GameObject sparkleGO = new GameObject("SparkleParticles");
        sparkleGO.transform.SetParent(transform);
        sparkleGO.transform.localPosition = particleOffset;
        
        // Add ParticleSystem component
        sparkleParticles = sparkleGO.AddComponent<ParticleSystem>();
        
        // Configure sparkle particles
        var main = sparkleParticles.main;
        main.startLifetime = sparkleParticleLifetime;
        main.startSpeed = sparkleSpeed;
        main.startColor = sparkleColor;
        main.startSize = sparkleParticleSize; // Set particle size
        main.maxParticles = 20; // Reduced from 30
        
        var emission = sparkleParticles.emission;
        emission.rateOverTime = sparkleEmissionRate;
        
        var shape = sparkleParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.08f; // Reduced from 0.15f
        
        var colorOverLifetime = sparkleParticles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(sparkleColor, 0.0f), 
                new GradientColorKey(sparkleColor * 1.2f, 0.5f), // Use sparkleColor instead of hardcoded white
                new GradientColorKey(sparkleColor, 1.0f) 
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(0.0f, 0.0f), 
                new GradientAlphaKey(1.0f, 0.5f), 
                new GradientAlphaKey(0.0f, 1.0f) 
            }
        );
        colorOverLifetime.color = gradient;
        
        Debug.Log("ParticleManager: Sparkle particles created");
    }
    
    void UpdateParticlePositions()
    {
        // Fire particles follow player with offset
        if (fireParticles != null)
        {
            fireParticles.transform.position = playerTransform.position + particleOffset;
        }
        
        // Sparkle particles follow player with offset
        if (sparkleParticles != null)
        {
            sparkleParticles.transform.position = playerTransform.position + particleOffset;
        }
    }
    
    /// <summary>
    /// Activa o desactiva las partículas
    /// </summary>
    public void SetParticlesActive(bool active)
    {
        if (fireParticles != null)
        {
            if (active)
                fireParticles.Play();
            else
                fireParticles.Stop();
        }
        
        if (sparkleParticles != null)
        {
            if (active)
                sparkleParticles.Play();
            else
                sparkleParticles.Stop();
        }
        
        Debug.Log($"ParticleManager: Particles set to {(active ? "active" : "inactive")}");
    }
    
    /// <summary>
    /// Cambia la intensidad de las partículas
    /// </summary>
    public void SetParticleIntensity(float intensity)
    {
        intensity = Mathf.Clamp01(intensity);
        
        if (fireParticles != null)
        {
            var emission = fireParticles.emission;
            emission.rateOverTime = fireEmissionRate * intensity;
        }
        
        if (sparkleParticles != null)
        {
            var emission = sparkleParticles.emission;
            emission.rateOverTime = sparkleEmissionRate * intensity;
        }
        
        Debug.Log($"ParticleManager: Particle intensity set to {intensity}");
    }
    
    /// <summary>
    /// Actualiza las partículas con los nuevos valores del Inspector
    /// </summary>
    public void UpdateParticleSettings()
    {
        if (fireParticles != null)
        {
            var main = fireParticles.main;
            main.startColor = fireColor;
            main.startSize = fireParticleSize;
            main.startSpeed = fireSpeed;
            main.startLifetime = fireParticleLifetime;
            
            var emission = fireParticles.emission;
            emission.rateOverTime = fireEmissionRate;
            
            // Update color over lifetime
            var colorOverLifetime = fireParticles.colorOverLifetime;
            if (colorOverLifetime.enabled)
            {
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { 
                        new GradientColorKey(fireColor, 0.0f), 
                        new GradientColorKey(fireColor * 0.8f, 1.0f)
                    },
                    new GradientAlphaKey[] { 
                        new GradientAlphaKey(1.0f, 0.0f), 
                        new GradientAlphaKey(0.0f, 1.0f) 
                    }
                );
                colorOverLifetime.color = gradient;
            }
        }
        
        if (sparkleParticles != null)
        {
            var main = sparkleParticles.main;
            main.startColor = sparkleColor;
            main.startSize = sparkleParticleSize;
            main.startSpeed = sparkleSpeed;
            main.startLifetime = sparkleParticleLifetime;
            
            var emission = sparkleParticles.emission;
            emission.rateOverTime = sparkleEmissionRate;
            
            // Update color over lifetime
            var colorOverLifetime = sparkleParticles.colorOverLifetime;
            if (colorOverLifetime.enabled)
            {
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { 
                        new GradientColorKey(sparkleColor, 0.0f), 
                        new GradientColorKey(sparkleColor * 1.2f, 0.5f),
                        new GradientColorKey(sparkleColor, 1.0f) 
                    },
                    new GradientAlphaKey[] { 
                        new GradientAlphaKey(0.0f, 0.0f), 
                        new GradientAlphaKey(1.0f, 0.5f), 
                        new GradientAlphaKey(0.0f, 1.0f) 
                    }
                );
                colorOverLifetime.color = gradient;
            }
        }
        
        Debug.Log("ParticleManager: Particle settings updated from Inspector");
    }
}
