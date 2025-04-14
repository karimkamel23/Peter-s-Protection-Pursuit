using UnityEngine;

public class ArrowTrapController : MonoBehaviour
{
    [SerializeField] private ArrowTrapModel model;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;

    private void Attack()
    {
        model.ResetCooldown();

        // Play sound directly
        if (model.ArrowSound != null)
        {
            SoundManager.instance.PlaySound(model.ArrowSound);
        }
        
        // Find and activate arrow
        int arrowIndex = FindArrow();
        arrows[arrowIndex].transform.position = firePoint.position;
        arrows[arrowIndex].GetComponent<EnemyProjectileController>().ActivateProjectile();
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy) return i;
        }
        return 0;
    }

    private void Update()
    {
        model.CooldownTimer += Time.deltaTime;
        
        if (model.IsReadyToAttack)
        {
            Attack();
        }
    }
} 