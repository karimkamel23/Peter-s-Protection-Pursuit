using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private CameraController cam;
    
    private DoorModel model;
    private SoundManager soundManager;
    
    private void Awake()
    {
        model = GetComponent<DoorModel>();
    }
    
    private void Start()
    {
        soundManager = SoundManager.instance;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Determine which room the player is coming from
            if (collision.transform.position.x < transform.position.x)
            {
                // Player is entering from the left
                if (cam != null)
                {
                    cam.MoveTooNewRoom(model.NextRoom);
                }
                
                // Activate/deactivate rooms
                model.NextRoom.GetComponent<RoomController>().ActivateRoom(true);
                model.PreviousRoom.GetComponent<RoomController>().ActivateRoom(false);
            }
            else
            {
                // Player is entering from the right
                if (cam != null)
                {
                    cam.MoveTooNewRoom(model.PreviousRoom);
                }
                
                // Activate/deactivate rooms
                model.NextRoom.GetComponent<RoomController>().ActivateRoom(false);
                model.PreviousRoom.GetComponent<RoomController>().ActivateRoom(true);
            }
        }
    }
    
    public void OpenDoor()
    {
        // Play sound effect
        if (soundManager != null && model.DoorOpenSound != null)
        {
            soundManager.PlaySound(model.DoorOpenSound);
        }
        
        // Update model
        model.OpenDoor();
    }
} 