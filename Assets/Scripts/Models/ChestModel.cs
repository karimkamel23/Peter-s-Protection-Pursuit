using UnityEngine;
using System;

public class ChestModel : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private bool isEnemy;

    private bool isOpen = false;

    // Events
    public event Action OnChestOpened;

    // Properties
    public bool IsOpen => isOpen;
    public bool IsEnemy => isEnemy;
    public GameObject ObjectToSpawn => objectToSpawn;

    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;
            
            // Activate the spawned object
            if (objectToSpawn != null)
            {
                objectToSpawn.SetActive(true);
            }
            
            // Notify listeners
            OnChestOpened?.Invoke();
        }
    }
} 