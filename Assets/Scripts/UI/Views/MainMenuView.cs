using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

[System.Serializable]
public class LevelData
{
    public string levelScene;                        // Scene name to load
    public string levelName;                        // Display name for the level
    public GameObject selectionIndicator;           // Reference to the LevelSelectIndicator inside it
}

public class MainMenuView : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private PlayerUIAnimation playerAnimation;

    [Header("Level Select")]
    [SerializeField] private GameObject levelSelectScreen;
    [SerializeField] private LevelData[] levels;
    [SerializeField] private TextMeshProUGUI selectedLevelTitle;
    
    [Header("Audio Screen")]
    [SerializeField] private GameObject audioScreen;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private TextMeshProUGUI soundPercentage;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicPercentage;
    
    [Header("Enemy Info")]
    [SerializeField] private GameObject enemyInfoScreen;
    
    private UIManager uiManager;
    private string selectedSceneName = null;
    
    private void Start()
    {
        uiManager = UIManager.Instance;
        
        // Configure sliders
        if (soundSlider != null)
            soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
        
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        
        // Initialize screens
        if (audioScreen != null)
            audioScreen.SetActive(false);
            
        if (enemyInfoScreen != null)
            enemyInfoScreen.SetActive(false);
        
        UpdateVolumeText();
    }
    
    private void Update()
    {
        // Update audio text if screens are active
        if (audioScreen != null && audioScreen.activeInHierarchy)
        {
            UpdateVolumeText();
        }
    }
    
    // Audio Controls
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
        }
    }
    
    // Screen Management
    public void OpenAudioScreen()
    {
        if (audioScreen != null)
        {
            audioScreen.SetActive(true);
            UpdateSliderValues();
            UpdateVolumeText();
        }
    }
    
    public void CloseAudioScreen()
    {
        if (audioScreen != null)
        {
            audioScreen.SetActive(false);
        }
    }
    
    public void ToggleEnemyInfoScreen()
    {
        if (enemyInfoScreen != null)
        {
            enemyInfoScreen.SetActive(!enemyInfoScreen.activeInHierarchy);
        }
    }
    
    // Navigation Methods
    public void GoToLevelSelect()
    {
        if (playerAnimation != null)
        {
            playerAnimation.PlayPlayerAnimation();
        }
        StartCoroutine(SwitchScreen(mainMenuScreen, levelSelectScreen));
    }
    
    public void GoToMainMenu()
    {
        if (playerAnimation != null)
        {
            playerAnimation.PlayReverseAnimation();
        }
        StartCoroutine(SwitchScreen(levelSelectScreen, mainMenuScreen));
    }
    
    private IEnumerator SwitchScreen(GameObject screenToHide, GameObject screenToShow)
    {
        if (screenToHide != null)
            screenToHide.SetActive(false);
            
        yield return new WaitForSeconds(2.5f);
        
        if (screenToShow != null)
            screenToShow.SetActive(true);
    }
    
    // Level Selection
    public void SelectLevel(int index)
    {
        // Sanity check
        if (index < 0 || index >= levels.Length) return;

        // Deactivate all selection indicators
        foreach (var level in levels)
        {
            if (level.selectionIndicator != null)
                level.selectionIndicator.SetActive(false);
        }

        // Activate the selected one
        var selectedLevel = levels[index];
        if (selectedLevel.selectionIndicator != null)
            selectedLevel.selectionIndicator.SetActive(true);

        // Set the selected scene name
        selectedSceneName = selectedLevel.levelScene;

        // Update title text
        if (selectedLevelTitle != null)
            selectedLevelTitle.text = $"{selectedLevel.levelName}";
    }
    
    public void PlaySelectedLevel()
    {
        if (!string.IsNullOrEmpty(selectedSceneName))
        {
            if (levelSelectScreen != null)
                levelSelectScreen.SetActive(false);
                
            if (playerAnimation != null)
                playerAnimation.PlayPlayerAnimation();
                
            StartCoroutine(LoadSelectedLevel());
        }
    }
    
    private IEnumerator LoadSelectedLevel()
    {
        yield return new WaitForSeconds(1.5f);
        SceneLoader.Instance.LoadScene(selectedSceneName);
    }
    
    // General Navigation
    public void ReturnToMainMenu()
    {
        if (uiManager != null)
        {
            uiManager.MainMenu();
        }
    }
} 