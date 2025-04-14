using UnityEngine;

[System.Serializable]
public class KeyModel
{
    [SerializeField] private AudioClip collectSound;
    
    // Properties
    public AudioClip CollectSound => collectSound;
} 