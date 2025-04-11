using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private ProjectileModel model;
    private ProjectileView view;
    private SoundManager soundManager;

    private void Awake()
    {
        model = GetComponent<ProjectileModel>();
        view = GetComponent<ProjectileView>();
    }

    private void Start()
    {
        soundManager = SoundManager.instance;
        
        if (model != null)
        {
            model.OnProjectileHit += HandleHit;
            model.OnProjectileDeactivate += model.Deactivate;
        }
    }

    private void Update()
    {
        if (model.IsHit) return;

        // Update movement
        float movementAmount = model.Speed * Time.deltaTime * model.Direction;
        view.UpdatePosition(movementAmount);

        // Update lifetime
        model.UpdateLifetime(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore certain collisions
        if (collision.CompareTag("Door") || collision.CompareTag("Collectible")) return;
        
        // Register hit and handle visual effects
        model.RegisterHit();
        
        // Handle damage to enemy
        if (collision.CompareTag("Enemy"))
        {
            HealthModel healthModel = collision.GetComponent<HealthModel>();
            if (healthModel != null)
            {
                healthModel.TakeDamage(1);
            }
        }
    }

    private void HandleHit()
    {
        if (soundManager != null && model.ImpactSound != null)
        {
            soundManager.PlaySound(model.ImpactSound);
        }
    }

    public void SetDirection(float direction)
    {
        // Reset state
        gameObject.SetActive(true);
        model.Reset();
        model.SetDirection(direction);
        
        // Update visuals
        view.UpdateVisualDirection(direction);
    }

    private void OnDestroy()
    {
        if (model != null)
        {
            model.OnProjectileHit -= HandleHit;
            model.OnProjectileDeactivate -= model.Deactivate;
        }
    }
} 