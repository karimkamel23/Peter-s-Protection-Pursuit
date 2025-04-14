using UnityEngine;
using System;

public class HealthModel : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    private int currentHealth;
    private bool invulnerable;
    private bool dead;

    // Event for UI updates
    public event Action<int, int> OnHealthChanged; // (currentHealth, maxHealth)

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Notify subscribers of initial health
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // Properties
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => dead;
    public bool IsInvulnerable 
    { 
        get => invulnerable; 
        set => invulnerable = value; 
    }

    // Methods to modify health state
    public void SetHealth(int value)
    {
        int previousHealth = currentHealth;
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        
        if (previousHealth != currentHealth)
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            
            if (currentHealth <= 0 && !dead)
            {
                dead = true;
            }
        }
    }
} 