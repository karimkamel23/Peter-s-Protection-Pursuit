using UnityEngine;

public class ButtonPlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private AudioClip projectileSound;

    private Animator anim;
    private ButtonPlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private bool attackRequested = false;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<ButtonPlayerMovement>();
    }


    private void Update()
    {
        if (attackRequested && cooldownTimer > attackCooldown && playerMovement.CanAttack()) { 
            Attack();
            attackRequested = false;
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(projectileSound);
        anim.SetTrigger("Attack");
        cooldownTimer = 0;

        projectiles[FindProjectile()].transform.position = firePoint.position;
        projectiles[FindProjectile()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    public void AttackRequest() => attackRequested = true;
    private int FindProjectile()
    {

        for (int i = 0; i < projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy) return i;
        }

        return 0;
    }

}
