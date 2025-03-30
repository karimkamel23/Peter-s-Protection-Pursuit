using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private bool top;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;

    private Animator anim;
    private EnemyPatrol enemyPatrol;
    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {

        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && cooldownTimer >= attackCooldown && playerHealth.GetCurrentHealth()>0)
        {
            cooldownTimer = 0;
            anim.SetTrigger("attack");
            SoundManager.instance.PlaySound(attackSound);

        }

        if(enemyPatrol != null) 
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        // Adjust the BoxCast size: only increase height at the **top** if "top" is true
        float boxHeight = boxCollider.bounds.size.y;
        float adjustedHeight = top ? boxHeight + (boxHeight * 0.1f) : boxHeight; // Only increases the top area

        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, adjustedHeight);
        Vector2 boxOrigin = (Vector2)boxCollider.bounds.center + (Vector2)(transform.right * range * transform.localScale.x * colliderDistance);

        //RaycastHit2D hit = Physics2D.BoxCast(
        //    boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        //    new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * topMultiplier, boxCollider.bounds.size.z),
        //    0, Vector2.left, 0, playerLayer);

        RaycastHit2D hit = Physics2D.BoxCast(
            boxOrigin, boxSize,
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Adjust the BoxCast size: only increase height at the **top** if "top" is true
        float boxHeight = boxCollider.bounds.size.y;
        float adjustedHeight = top ? boxHeight + (boxHeight * 0.1f) : boxHeight; // Only increases the top area

        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, adjustedHeight);
        Vector2 boxOrigin = (Vector2)boxCollider.bounds.center + (Vector2)(transform.right * range * transform.localScale.x * colliderDistance);

        Gizmos.DrawWireCube(boxOrigin, boxSize);
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
