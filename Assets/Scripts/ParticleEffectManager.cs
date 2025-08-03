using UnityEngine;
using System.Collections.Generic;

public class ParticleEffectManager : MonoBehaviour
{
    [Header("Particle Effects")]
    public ParticleSystem collectEffect;
    public ParticleSystem damageEffect;
    public ParticleSystem explosionEffect;
    public ParticleSystem sparkleEffect;
    
    [Header("Settings")]
    public int maxActiveEffects = 10;
    public bool autoCleanup = true;
    public float cleanupDelay = 2f;
    
    // Singleton instance
    public static ParticleEffectManager Instance { get; private set; }
    
    // Private variables
    private Queue<ParticleSystem> activeEffects = new Queue<ParticleSystem>();
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayCollectEffect(Vector3 position)
    {
        PlayEffect(collectEffect, position);
    }
    
    public void PlayDamageEffect(Vector3 position)
    {
        PlayEffect(damageEffect, position);
    }
    
    public void PlayExplosionEffect(Vector3 position)
    {
        PlayEffect(explosionEffect, position);
    }
    
    public void PlaySparkleEffect(Vector3 position)
    {
        PlayEffect(sparkleEffect, position);
    }
    
    void PlayEffect(ParticleSystem effectPrefab, Vector3 position)
    {
        if (effectPrefab == null) return;
        
        // Create effect instance
        ParticleSystem effect = Instantiate(effectPrefab, position, Quaternion.identity);
        
        // Add to active effects queue
        activeEffects.Enqueue(effect);
        
        // Play the effect
        effect.Play();
        
        // Cleanup if too many effects
        if (activeEffects.Count > maxActiveEffects)
        {
            ParticleSystem oldEffect = activeEffects.Dequeue();
            if (oldEffect != null)
            {
                Destroy(oldEffect.gameObject);
            }
        }
        
        // Auto cleanup
        if (autoCleanup)
        {
            Destroy(effect.gameObject, cleanupDelay);
        }
    }
    
    public void PlayEffectWithRotation(ParticleSystem effectPrefab, Vector3 position, Quaternion rotation)
    {
        if (effectPrefab == null) return;
        
        // Create effect instance with rotation
        ParticleSystem effect = Instantiate(effectPrefab, position, rotation);
        
        // Add to active effects queue
        activeEffects.Enqueue(effect);
        
        // Play the effect
        effect.Play();
        
        // Cleanup if too many effects
        if (activeEffects.Count > maxActiveEffects)
        {
            ParticleSystem oldEffect = activeEffects.Dequeue();
            if (oldEffect != null)
            {
                Destroy(oldEffect.gameObject);
            }
        }
        
        // Auto cleanup
        if (autoCleanup)
        {
            Destroy(effect.gameObject, cleanupDelay);
        }
    }
    
    public void PlayEffectWithScale(ParticleSystem effectPrefab, Vector3 position, Vector3 scale)
    {
        if (effectPrefab == null) return;
        
        // Create effect instance
        ParticleSystem effect = Instantiate(effectPrefab, position, Quaternion.identity);
        
        // Apply scale
        effect.transform.localScale = scale;
        
        // Add to active effects queue
        activeEffects.Enqueue(effect);
        
        // Play the effect
        effect.Play();
        
        // Cleanup if too many effects
        if (activeEffects.Count > maxActiveEffects)
        {
            ParticleSystem oldEffect = activeEffects.Dequeue();
            if (oldEffect != null)
            {
                Destroy(oldEffect.gameObject);
            }
        }
        
        // Auto cleanup
        if (autoCleanup)
        {
            Destroy(effect.gameObject, cleanupDelay);
        }
    }
    
    public void ClearAllEffects()
    {
        while (activeEffects.Count > 0)
        {
            ParticleSystem effect = activeEffects.Dequeue();
            if (effect != null)
            {
                Destroy(effect.gameObject);
            }
        }
    }
    
    public void SetMaxActiveEffects(int max)
    {
        maxActiveEffects = max;
        
        // Cleanup excess effects
        while (activeEffects.Count > maxActiveEffects)
        {
            ParticleSystem effect = activeEffects.Dequeue();
            if (effect != null)
            {
                Destroy(effect.gameObject);
            }
        }
    }
    
    public void SetAutoCleanup(bool enabled)
    {
        autoCleanup = enabled;
    }
    
    public void SetCleanupDelay(float delay)
    {
        cleanupDelay = delay;
    }
    
    // Static methods for easy access
    public static void PlayCollectEffectStatic(Vector3 position)
    {
        if (Instance != null)
        {
            Instance.PlayCollectEffect(position);
        }
    }
    
    public static void PlayDamageEffectStatic(Vector3 position)
    {
        if (Instance != null)
        {
            Instance.PlayDamageEffect(position);
        }
    }
    
    public static void PlayExplosionEffectStatic(Vector3 position)
    {
        if (Instance != null)
        {
            Instance.PlayExplosionEffect(position);
        }
    }
    
    public static void PlaySparkleEffectStatic(Vector3 position)
    {
        if (Instance != null)
        {
            Instance.PlaySparkleEffect(position);
        }
    }
    
    public static void ClearAllEffectsStatic()
    {
        if (Instance != null)
        {
            Instance.ClearAllEffects();
        }
    }
} 