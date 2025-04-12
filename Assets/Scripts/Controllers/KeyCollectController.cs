using UnityEngine;

public class KeyCollectController : MonoBehaviour
{
    private KeyModel model;
    private KeyView view;
    private SoundManager soundManager;
    
    private void Awake()
    {
        model = GetComponent<KeyModel>();
        view = FindObjectOfType<KeyView>();
    }
    
    private void Start()
    {
        soundManager = SoundManager.instance;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Play sound effect
            if (soundManager != null && model != null && model.CollectSound != null)
            {
                soundManager.PlaySound(model.CollectSound);
            }
            
            // Update view
            if (view != null)
            {
                view.CollectKey();
            }
        }
    }
} 