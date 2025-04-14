using UnityEngine;
using System;

public class ProjectileModel : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private AudioClip impactSound;

    private bool hit;
    private float direction;
    private float lifetime;
    private BoxCollider2D boxCollider;

    // Events
    public event Action OnProjectileHit;
    public event Action OnProjectileDeactivate;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Reset()
    {
        lifetime = 0;
        hit = false;
        boxCollider.enabled = true;
    }

    public void SetDirection(float newDirection)
    {
        direction = newDirection;
    }

    public void RegisterHit()
    {
        if (!hit)
        {
            hit = true;
            boxCollider.enabled = false;
            OnProjectileHit?.Invoke();
        }
    }

    public void UpdateLifetime(float deltaTime)
    {
        if (hit) return;

        lifetime += deltaTime;
        if (lifetime > maxLifetime)
        {
            OnProjectileDeactivate?.Invoke();
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // Properties
    public float Speed => speed;
    public float Direction => direction;
    public bool IsHit => hit;
    public AudioClip ImpactSound => impactSound;
} 