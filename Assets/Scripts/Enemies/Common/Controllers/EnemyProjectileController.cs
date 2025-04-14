using UnityEngine;

public class EnemyProjectileController : EnemyDamage
{
    [SerializeField] private EnemyProjectileModel model;
    private EnemyProjectileView view;
    private BoxCollider2D coll;

    private void Awake()
    {
        view = GetComponent<EnemyProjectileView>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        model.Reset();
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if (model.Hit) return;
        
        // Move the projectile
        float movementSpeed = model.Speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        // Update lifetime and check if it should deactivate
        model.LifeTime += Time.deltaTime;
        if (model.LifeTime > model.ResetTime) 
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door") || collision.CompareTag("Collectible")) return;
        
        model.Hit = true;
        base.OnTriggerEnter2D(collision);
        coll.enabled = false;

        view.PlayExplodeAnimation();
        view.PlayImpactSound(model.ImpactSound);
    }

    // Animation event callback
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
} 