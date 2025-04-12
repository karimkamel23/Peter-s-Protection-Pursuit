using UnityEngine;

public class InfoTriggerController : MonoBehaviour
{
    private InfoModel model;
    private SoundManager soundManager;
    private bool hasBeenTriggered = false;
    
    private void Awake()
    {
        model = GetComponent<InfoModel>();
    }
    
    private void Start()
    {
        soundManager = SoundManager.instance;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasBeenTriggered && collision.CompareTag("Player") && model != null)
        {
            // Play sound effect
            if (soundManager != null && model.CollectSound != null)
            {
                soundManager.PlaySound(model.CollectSound);
            }
            
            // Trigger info display via model
            model.TriggerInfo();
            
            // Deactivate gameObject (optional - could be controlled by property)
            hasBeenTriggered = true;
            gameObject.SetActive(false);
        }
    }
} 