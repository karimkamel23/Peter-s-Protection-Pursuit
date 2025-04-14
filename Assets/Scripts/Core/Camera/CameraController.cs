using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room Camera
    [Header("Room Camera")]
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //Follow Player
    [Header("Follow Player Camera")]
    [SerializeField] bool followPlayer = true;
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;


    private void Update()
    {
        if (followPlayer)
        {
            transform.position = new Vector3(player.position.x + lookAhead, player.position.y, transform.position.z);
            lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed); //Follow Player
        }
        else //Room Camera
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y,
                transform.position.z), ref velocity, speed);


    }

    public void MoveTooNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
