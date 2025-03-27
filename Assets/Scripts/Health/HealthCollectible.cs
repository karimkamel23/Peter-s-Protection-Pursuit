using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private int healthValue = 1 ;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().Heal(healthValue);
            gameObject.SetActive(false);
        }
    }
}
