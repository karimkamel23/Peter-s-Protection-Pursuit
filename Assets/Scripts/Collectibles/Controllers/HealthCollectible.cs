using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private int healthValue = 1 ;
    [Header("SFX")]
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HealthController>().Heal(healthValue);
            SoundManager.instance.PlaySound(pickupSound);
            gameObject.SetActive(false);
        }
    }
}
