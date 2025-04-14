using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3; // Will be transferred to model

    [Header("Components")]
    [SerializeField] private Behaviour[] componentsToDisable;
    
    [Header("Iframes")]
    [SerializeField] private float iFramesDuration = 1f;
    [SerializeField] private int numberOfFlashes = 4;

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private HealthModel healthModel;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private SoundManager soundManager;

    private void Awake()
    {
        healthModel = GetComponent<HealthModel>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
        // If no health model, create one
        if (healthModel == null)
        {
            healthModel = gameObject.AddComponent<HealthModel>();
        }
    }

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    public void TakeDamage(int damage)
    {
        if (healthModel.IsInvulnerable || healthModel.IsDead) return;

        // Update health in model
        healthModel.SetHealth(healthModel.CurrentHealth - damage);

        if (healthModel.CurrentHealth > 0)
        {
            // Hurt sequence
            anim.SetTrigger("hurt");
            if (soundManager != null && hurtSound != null)
            {
                soundManager.PlaySound(hurtSound);
            }
            StartCoroutine(Invulnerability());
        }
        else
        {
            // Death sequence
            anim.SetTrigger("death");
            
            foreach (Behaviour component in componentsToDisable)
            {
                if (component != null)
                {
                    component.enabled = false;
                }
            }
            
            if (soundManager != null && deathSound != null)
            {
                soundManager.PlaySound(deathSound);
            }
        }
    }

    public void Heal(int amount)
    {
        if (healthModel.IsDead) return;

        healthModel.SetHealth(healthModel.CurrentHealth + amount);
    }

    private IEnumerator Invulnerability()
    {
        healthModel.IsInvulnerable = true;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        
        Physics2D.IgnoreLayerCollision(8, 9, false);
        healthModel.IsInvulnerable = false;
    }

    // Called by animation events when death animation completes
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // Public methods for external access
    public int GetCurrentHealth()
    {
        return healthModel.CurrentHealth;
    }
} 