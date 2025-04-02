using UnityEngine;

public class InfoTrigger : MonoBehaviour
{
    [SerializeField] private string infoText;
    [SerializeField] private AudioClip collectSound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(collectSound);
            InfoUIManager.Instance.ShowInfo(infoText);
            gameObject.SetActive(false);
        }
    }
}
