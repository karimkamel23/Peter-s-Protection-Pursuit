using UnityEngine;

public class RoomModel : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    
    private Vector3[] initialPositions;
    private bool isActive = false;
    
    private void Awake()
    {
        // Store initial positions of all enemies
        initialPositions = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                initialPositions[i] = enemies[i].transform.position;
            }
        }
    }
    
    // Properties
    public GameObject[] Enemies => enemies;
    public Vector3[] InitialPositions => initialPositions;
    public bool IsActive 
    {
        get => isActive;
        set => isActive = value;
    }
} 