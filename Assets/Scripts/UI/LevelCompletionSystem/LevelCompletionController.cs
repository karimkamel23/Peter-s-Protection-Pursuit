using UnityEngine;

public class LevelCompletionController : MonoBehaviour
{
    private LevelCompletionModel model;
    
    private void Awake()
    {
        // Find or create model
        model = FindObjectOfType<LevelCompletionModel>();
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && model != null)
        {
            model.CompleteLevel();
        }
    }
} 