using UnityEngine;
using System;

public class DoorModel : MonoBehaviour
{
    [Header("Room References")]
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    
    [Header("Lock Settings")]
    [SerializeField] private AudioClip doorOpenSound;
    
    private bool isOpen = false;
    
    // Events
    public event Action OnDoorOpened;
    
    // Properties
    public Transform PreviousRoom => previousRoom;
    public Transform NextRoom => nextRoom;
    public AudioClip DoorOpenSound => doorOpenSound;
    public bool IsOpen
    {
        get => isOpen;
        private set
        {
            if (isOpen != value)
            {
                isOpen = value;
                if (isOpen)
                {
                    OnDoorOpened?.Invoke();
                }
            }
        }
    }
    
    public void OpenDoor()
    {
        IsOpen = true;
    }
} 