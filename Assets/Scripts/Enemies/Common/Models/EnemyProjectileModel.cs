using UnityEngine;

[System.Serializable]
public class EnemyProjectileModel
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private AudioClip impactSound;

    // State
    private float lifeTime;
    private bool hit;

    public float Speed => speed;
    public float ResetTime => resetTime;
    public AudioClip ImpactSound => impactSound;
    
    public float LifeTime 
    { 
        get => lifeTime; 
        set => lifeTime = value; 
    }
    
    public bool Hit 
    { 
        get => hit; 
        set => hit = value; 
    }

    public void Reset()
    {
        hit = false;
        lifeTime = 0;
    }
} 