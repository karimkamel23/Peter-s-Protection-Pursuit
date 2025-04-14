using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    [SerializeField] private RangedEnemyModel model;
    [SerializeField] private BoxCollider2D boxCollider;

    private Animator anim;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        model.CooldownTimer += Time.deltaTime;

        if (PlayerInSight() && model.CooldownTimer >= model.AttackCooldown)
        {
            model.CooldownTimer = 0;
            anim.SetTrigger("attack");
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private void RangedAttack()
    {
        SoundManager.instance.PlaySound(model.ProjectileSound);
        model.CooldownTimer = 0;
        
        int projectileIndex = FindProjectile();
        model.Projectiles[projectileIndex].transform.position = model.FirePoint.position;
        model.Projectiles[projectileIndex].GetComponent<EnemyProjectileController>().ActivateProjectile();
    }

    private int FindProjectile()
    {
        for (int i = 0; i < model.Projectiles.Length; i++)
        {
            if (!model.Projectiles[i].activeInHierarchy) return i;
        }

        return 0;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * model.Range * transform.localScale.x * model.ColliderDistance,
            new Vector3(boxCollider.bounds.size.x * model.Range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, model.PlayerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null || model == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * model.Range * transform.localScale.x * model.ColliderDistance,
            new Vector3(boxCollider.bounds.size.x * model.Range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
} 