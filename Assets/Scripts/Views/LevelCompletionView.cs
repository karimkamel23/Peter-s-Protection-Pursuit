using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private GameObject nextLevelButton;
    
    [Header("Audio")]
    [SerializeField] private AudioClip levelCompleteSound;
    
    private LevelCompletionModel model;
    private SoundManager soundManager;
    private UIManager uiManager;
    private LoadingScreen loadingScreen;
    
    private void Start()
    {
        // Find required references
        model = FindObjectOfType<LevelCompletionModel>();
        soundManager = SoundManager.instance;
        uiManager = FindObjectOfType<UIManager>();
        loadingScreen = LoadingScreen.Instance;
        
        // Hide the completion screen initially
        if (levelCompleteScreen != null)
        {
            levelCompleteScreen.SetActive(false);
        }
        
        // Subscribe to level completion event
        if (model != null)
        {
            model.OnLevelCompleted += HandleLevelCompleted;
        }
    }
    
    private void HandleLevelCompleted()
    {
        // Show level complete screen
        if (levelCompleteScreen != null)
        {
            levelCompleteScreen.SetActive(true);
            
            // Configure next level button AFTER screen is activated
            if (nextLevelButton != null)
            {
                nextLevelButton.SetActive(HasNextLevel());
            }
        }
        
        // Play sound effect
        if (soundManager != null && levelCompleteSound != null)
        {
            soundManager.PlaySound(levelCompleteSound);
        }
        
        // Hide HUD using UIManager
        if (uiManager != null)
        {
            uiManager.ToggleHUD(false);
        }
    }
    
    // Button action methods
    public void LoadNextLevel()
    {
        if (HasNextLevel())
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            if (loadingScreen != null)
            {
                loadingScreen.LoadScene(nextIndex);
            }
            else
            {
                SceneManager.LoadScene(nextIndex);
            }
        }
    }
    
    public void ReturnToHome()
    {
        if (uiManager != null)
        {
            uiManager.MainMenu();
        }
    }
    
    public void RestartLevel()
    {
        if (uiManager != null)
        {
            uiManager.Restart();
        }
    }
    
    // Helper methods
    private bool HasNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        return (currentIndex + 1 < totalScenes);
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        if (model != null)
        {
            model.OnLevelCompleted -= HandleLevelCompleted;
        }
    }
} 