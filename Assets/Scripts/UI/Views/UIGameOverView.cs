using UnityEngine;

public class UIGameOverView : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    private SoundManager soundManager;
    private UIManager uiManager;


    private void Awake()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    private void Start()
    {
        soundManager = SoundManager.instance;
        uiManager = UIManager.Instance;
        // Find and subscribe to player's game over event
        PlayerModel playerModel = FindObjectOfType<PlayerModel>();
        if (playerModel != null)
        {
            playerModel.OnPlayerGameOver += HandleGameOver;
        }
    }

    private void HandleGameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        if (soundManager != null && gameOverSound != null)
        {
            soundManager.PlaySound(gameOverSound);
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

    private void OnDestroy()
    {
        // Unsubscribe from player's game over event
        PlayerModel playerModel = FindObjectOfType<PlayerModel>();
        if (playerModel != null)
        {
            playerModel.OnPlayerGameOver -= HandleGameOver;
        }
    }
} 