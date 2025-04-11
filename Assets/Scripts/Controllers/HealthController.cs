using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Behaviour[] disableOnDeath;
    
    [Header("Iframes")]
    [SerializeField] private float iFramesDuration = 1f;
    [SerializeField] private int numberOfFlashes = 4;
    [SerializeField] private bool useIFrames = true;

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private HealthModel healthModel;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private SoundManager soundManager;

    private void Awake()
    {
        healthModel = GetComponent<HealthModel>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        soundManager = SoundManager.instance;
        
        // Subscribe to health model events
        if (healthModel != null)
        {
            healthModel.OnDamaged += HandleDamage;
            healthModel.OnDeath += HandleDeath;
        }
    }

    private void HandleDamage()
    {
        if (animator != null)
        {
            animator.SetTrigger("hurt");
        }

        if (soundManager != null && hurtSound != null)
        {
            soundManager.PlaySound(hurtSound);
        }

        if (useIFrames)
        {
            StartCoroutine(InvulnerabilityFrames());
        }
    }

    private void HandleDeath()
    {
        if (animator != null)
        {
            animator.SetTrigger("death");
        }

        if (soundManager != null && deathSound != null)
        {
            soundManager.PlaySound(deathSound);
        }

        // Disable components
        foreach (Behaviour component in disableOnDeath)
        {
            if (component != null)
            {
                component.enabled = false;
            }
        }
    }

    private IEnumerator InvulnerabilityFrames()
    {
        // Set invulnerable state
        healthModel.SetInvulnerable(true);
        
        // Enable physics layer collision ignore (typically player layer 8 with enemy/damage layer 9)
        Physics2D.IgnoreLayerCollision(8, 9, true);

        // Flash the sprite
        if (spriteRenderer != null)
        {
            for (int i = 0; i < numberOfFlashes; i++)
            {
                spriteRenderer.color = new Color(1, 0, 0, 0.5f);
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
                
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            }
        }
        else
        {
            yield return new WaitForSeconds(iFramesDuration);
        }

        // Disable invulnerability
        Physics2D.IgnoreLayerCollision(8, 9, false);
        healthModel.SetInvulnerable(false);
    }

    private void OnDestroy()
    {
        // Unsubscribe from health model events
        if (healthModel != null)
        {
            healthModel.OnDamaged -= HandleDamage;
            healthModel.OnDeath -= HandleDeath;
        }
    }
} 