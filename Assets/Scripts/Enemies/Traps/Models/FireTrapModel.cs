using UnityEngine;

[System.Serializable]
public class FireTrapModel
{
    [SerializeField] private int damage;
    
    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    
    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    // State
    private bool triggered;
    private bool active;

    // Properties
    public int Damage => damage;
    public float ActivationDelay => activationDelay;
    public float ActiveTime => activeTime;
    public AudioClip FiretrapSound => firetrapSound;
    
    public bool Triggered 
    { 
        get => triggered; 
        set => triggered = value; 
    }
    
    public bool Active 
    { 
        get => active; 
        set => active = value; 
    }
} 