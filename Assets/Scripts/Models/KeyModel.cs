using UnityEngine;

public class KeyModel : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    
    // Properties
    public AudioClip CollectSound => collectSound;
} 