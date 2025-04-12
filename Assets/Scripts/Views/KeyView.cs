using UnityEngine;

public class KeyView : MonoBehaviour
{
    [SerializeField] private GameObject playerKeyHolder;
    
    public void CollectKey()
    {
        // Show the key on the player
        if (playerKeyHolder != null)
        {
            playerKeyHolder.SetActive(true);
        }
        
        // Hide the key in the world
        gameObject.SetActive(false);
    }
} 