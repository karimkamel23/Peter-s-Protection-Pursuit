using UnityEngine;
using System;

public class InfoModel : MonoBehaviour
{
    // Event that fires when info is triggered
    public event Action<string> OnInfoTriggered;
    
    // Collection of info entries (could be expanded to include IDs, etc.)
    [SerializeField] private string infoText;
    [SerializeField] private AudioClip collectSound;
    
    // Properties
    public string InfoText => infoText;
    public AudioClip CollectSound => collectSound;
    
    // Call this to trigger the info display
    public void TriggerInfo()
    {
        OnInfoTriggered?.Invoke(infoText);
    }
} 