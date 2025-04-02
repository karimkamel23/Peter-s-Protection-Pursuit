using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Lock Settings")]
    [SerializeField] private GameObject doorCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveTooNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().ActivateRoom(false);

            }
            else
            {
                cam.MoveTooNewRoom(previousRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
            }
        }
    }

    public void OpenDoor()
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (doorCollider != null)
            doorCollider.SetActive(false);
    }
}
