using UnityEngine;
using System;

public class HealthModel : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    private int currentHealth;
    private bool invulnerable;
    private bool dead;

    // Events
    public event Action<int, int> OnHealthChanged; // (currentHealth, maxHealth)
    public event Action OnDamaged;
    public event Action OnHealed;
    public event Action OnDeath;
    public event Action<bool> OnInvulnerabilityChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Notify subscribers of initial health
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetInvulnerable(bool value)
    {
        invulnerable = value;
        OnInvulnerabilityChanged?.Invoke(invulnerable);
    }
    
    public void TakeDamage(int damage)
    {
        if (invulnerable || dead) 
        {
            return;
        }
        
        int previousHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth != previousHealth)
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            OnDamaged?.Invoke();
        }

        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (dead)
        {
            return;
        }
        
        int previousHealth = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth != previousHealth)
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            OnHealed?.Invoke();
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsDead()
    {
        return dead;
    }

    public bool IsInvulnerable()
    {
        return invulnerable;
    }
} 