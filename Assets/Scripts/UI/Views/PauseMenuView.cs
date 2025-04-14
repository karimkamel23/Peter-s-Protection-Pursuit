using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseScreen;
    
    [Header("Audio Controls")]
    [SerializeField] private Slider soundSlider;
    [SerializeField] private TextMeshProUGUI soundPercentage;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicPercentage;
    
    [Header("Audio")]
    [SerializeField] private AudioClip pauseSound;
    
    private SoundManager soundManager;
    private UIManager uiManager;
    private bool isPaused = false;
    
    private void Start()
    {
        // Find required references
        soundManager = SoundManager.instance;
        uiManager = UIManager.Instance;
        
        // Hide the pause screen initially
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
        
        // Initialize sliders
        if (soundSlider != null && musicSlider != null)
        {
            soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }
    }
    
    private void Update()
    {
        if (isPaused)
        {
            UpdateVolumeText();
        }
    }
    
    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(isPaused);
        }
        
        // Play sound effect
        if (soundManager != null && pauseSound != null)
        {
            soundManager.PlaySound(pauseSound);
        }
        
        // Set time scale
        Time.timeScale = isPaused ? 0 : 1;
        
        // Update volume sliders directly in pause menu if shown
        if (isPaused)
        {
            UpdateSliderValues();
        }
    }
    
    private void UpdateSliderValues()
    {
        if (uiManager != null)
        {
            if (soundSlider != null)
            {
                soundSlider.value = uiManager.GetSoundVolume();
            }
            
            if (musicSlider != null)
            {
                musicSlider.value = uiManager.GetMusicVolume();
            }
            
            UpdateVolumeText();
        }
    }
    
    private void UpdateVolumeText()
    {
        if (soundPercentage != null && uiManager != null)
        {
            soundPercentage.text = Mathf.RoundToInt(uiManager.GetSoundVolume() * 100f) + "%";
        }
        
        if (musicPercentage != null && uiManager != null)
        {
            musicPercentage.text = Mathf.RoundToInt(uiManager.GetMusicVolume() * 100f) + "%";
        }
    }
    
    public void OnSoundSliderChanged(float value)
    {
        if (uiManager != null)
        {
            uiManager.SetSoundVolume(value);
            UpdateVolumeText();
        }
    }
    
    public void OnMusicSliderChanged(float value)
    {
        if (uiManager != null)
        {
            uiManager.SetMusicVolume(value);
            UpdateVolumeText();
        }
    }
    

    public void RestartLevel()
    {
        if (uiManager != null)
        {
            Time.timeScale = 1; // Ensure time scale is reset
            uiManager.Restart();
        }
    }
    
    public void ReturnToMainMenu()
    {
        if (uiManager != null)
        {
            Time.timeScale = 1; // Ensure time scale is reset
            uiManager.MainMenu();
        }
    }
} 