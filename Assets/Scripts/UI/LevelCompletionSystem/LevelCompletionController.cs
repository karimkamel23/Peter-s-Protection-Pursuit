using UnityEngine;

public class LevelCompletionController : MonoBehaviour
{
    private LevelCompletionModel model;
    
    [SerializeField] private int levelNumber = 0;
    
    private void Awake()
    {
        // Find or create model
        model = FindObjectOfType<LevelCompletionModel>();
        
        // If level number not set, use current scene index
        if (levelNumber == 0)
        {
            levelNumber = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && model != null)
        {
            // Save progress before completing level
            if (ScoreHandler.Instance != null)
            {
                ScoreHandler.Instance.SaveScoreForLevel(levelNumber);
            }
            
            model.CompleteLevel();
        }
    }
} 