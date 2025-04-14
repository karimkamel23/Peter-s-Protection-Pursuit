using UnityEngine;

public class RoomController : MonoBehaviour
{
    private RoomModel model;
    
    private void Awake()
    {
        model = GetComponent<RoomModel>();
    }
    
    public void ActivateRoom(bool status)
    {
        model.IsActive = status;
        
        // Activate/deactivate enemies and reset their positions
        for (int i = 0; i < model.Enemies.Length; i++)
        {
            if (model.Enemies[i] != null)
            {
                // Set activation state
                model.Enemies[i].SetActive(status);
                
                // Reset position if activating
                if (status)
                {
                    model.Enemies[i].transform.position = model.InitialPositions[i];
                }
            }
        }
    }
} 