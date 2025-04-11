using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected int damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Get the health component and apply damage if it exists
            HealthModel healthComponent = collision.GetComponent<HealthModel>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
            }
        }
    }
}
