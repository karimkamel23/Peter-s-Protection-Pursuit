using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    [SerializeField] private MeleeEnemyModel model;
    [SerializeField] private BoxCollider2D boxCollider;

    private Animator anim;
    private EnemyPatrol enemyPatrol;
    private HealthController playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        model.CooldownTimer += Time.deltaTime;

        if (PlayerInSight() && model.CooldownTimer >= model.AttackCooldown && playerHealth.GetCurrentHealth() > 0)
        {
            model.CooldownTimer = 0;
            anim.SetTrigger("attack");
            SoundManager.instance.PlaySound(model.AttackSound);
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        // Adjust the BoxCast size: only increase height at the top if "top" is true
        float boxHeight = boxCollider.bounds.size.y;
        float adjustedHeight = model.Top ? boxHeight + (boxHeight * 0.1f) : boxHeight; // Only increases the top area

        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * model.Range, adjustedHeight);
        Vector2 boxOrigin = (Vector2)boxCollider.bounds.center + (Vector2)(transform.right * model.Range * transform.localScale.x * model.ColliderDistance);

        RaycastHit2D hit = Physics2D.BoxCast(
            boxOrigin, boxSize,
            0, Vector2.left, 0, model.PlayerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<HealthController>();
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null || model == null) return;
        
        Gizmos.color = Color.red;
        // Adjust the BoxCast size: only increase height at the top if "top" is true
        float boxHeight = boxCollider.bounds.size.y;
        float adjustedHeight = model.Top ? boxHeight + (boxHeight * 0.1f) : boxHeight; // Only increases the top area

        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * model.Range, adjustedHeight);
        Vector2 boxOrigin = (Vector2)boxCollider.bounds.center + (Vector2)(transform.right * model.Range * transform.localScale.x * model.ColliderDistance);

        Gizmos.DrawWireCube(boxOrigin, boxSize);
    }

    // Animation event
    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(model.Damage);
        }
    }
} 