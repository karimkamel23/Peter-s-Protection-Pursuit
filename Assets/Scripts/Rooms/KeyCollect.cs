using UnityEngine;

public class KeyCollect : MonoBehaviour
{
    [SerializeField] private GameObject playerKeyHolder;
    [SerializeField] private AudioClip collectSound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(collectSound);
            playerKeyHolder.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
