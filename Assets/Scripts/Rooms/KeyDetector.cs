using UnityEngine;

public class KeyDetector : MonoBehaviour
{
    [SerializeField] private GameObject keyToDetect;
    [SerializeField] private Door doorToOpen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (keyToDetect.activeInHierarchy)
            {
                keyToDetect.SetActive(false);
                doorToOpen.OpenDoor();
            }
    }
}
