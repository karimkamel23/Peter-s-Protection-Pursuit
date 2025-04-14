using UnityEngine;

public class KeyDetector : MonoBehaviour
{
    [SerializeField] private GameObject keyToDetect;
    [SerializeField] private DoorController doorToOpen;

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
