using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

[System.Serializable]
public class LevelData
{
    public string levelScene;                        // Scene name to load
    public string levelName;                // Reference to the full Level1 or Level2 UI object
    public GameObject selectionIndicator;           // Reference to the LevelSelectIndicator inside it
}

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private AudioClip pauseSound;

    [Header("Level Completion")]
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private AudioClip levelCompleteSound;

    [Header("Other Screens")]
    [SerializeField] private GameObject[] screens;

    [Header("Audio")]
    [SerializeField] private GameObject audioScreen;
    [SerializeField] Slider soundSlider;
    [SerializeField] TextMeshProUGUI soundPercentage;
    [SerializeField] Slider musicSlider;
    [SerializeField] TextMeshProUGUI musicPercentage;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private PlayerUIAnimation playerAnimation;

    [Header("Level Select")]
    [SerializeField] private GameObject levelSelectScreen;
    [SerializeField] private LevelData[] levels;
    [SerializeField] private TextMeshProUGUI selectedLevelTitle;
    private string selectedSceneName = null;

    private void Awake()
    {
        if(gameOverScreen != null)
            gameOverScreen.SetActive(false);

        if (pauseScreen != null)
            pauseScreen.SetActive(false);

        if (levelCompleteScreen != null)
            pauseScreen.SetActive(false);
    }
    private void Update()
    {
        UpdateVolume();

    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void pauseGame()
    {
        pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
        SoundManager.instance.PlaySound(pauseSound);

        if (pauseScreen.activeInHierarchy) { 
            SetVolumeSliders();
            Time.timeScale = 0; 
        }
        else Time.timeScale = 1;
    }

    public void ToggleHUD(bool status)
    {
        foreach (GameObject screen in screens)
        {
            if (screen != null) screen.SetActive(status);
        }
    }

    public void LevelCompleted()
    {
        ToggleHUD(false);
        SoundManager.instance.PlaySound(levelCompleteSound);
        levelCompleteScreen.SetActive(true);
    }

    public void SetMusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(musicSlider.value);
    }

    public void SetSoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(soundSlider.value);
    }

    private void SetVolumeSliders()
    {
        soundSlider.value = SoundManager.instance.GetCurrentSoundVolume();
        musicSlider.value = SoundManager.instance.GetCurrentMusicVolume();
    }

    private void UpdateMusicPercentage()
    {
        musicPercentage.text = Mathf.RoundToInt(SoundManager.instance.GetCurrentMusicVolume() * 100f) + "%";
    }

    private void UpdateSoundPercentage()
    {
        soundPercentage.text = Mathf.RoundToInt(SoundManager.instance.GetCurrentSoundVolume() * 100f) + "%";
    }

    private void UpdateVolume()
    {
        UpdateMusicPercentage();
        UpdateSoundPercentage();
    }

    public void OpenAudioScreen()
    {
        audioScreen.gameObject.SetActive(true);
        SetVolumeSliders();
        UpdateVolume();
    }

    public void CloseAudioScreen()
    {
        audioScreen.gameObject.SetActive(false);
    }


    public void GoToLevelSelect()
    {
        playerAnimation.PlayPlayerAnimation();
        StartCoroutine(SwitchScreen(mainMenuScreen, levelSelectScreen));
    }

    public void GoToMainMenu()
    {
        playerAnimation.PlayReverseAnimation();
        StartCoroutine(SwitchScreen(levelSelectScreen, mainMenuScreen));
    }

    private IEnumerator SwitchScreen(GameObject screenToHide, GameObject screenToShow)
    {
        screenToHide.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        screenToShow.gameObject.SetActive(true);
    }

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
            levelSelectScreen.SetActive(false);
            playerAnimation.PlayPlayerAnimation();
            StartCoroutine(LoadSelectedLevel());
        }
    }

    private IEnumerator LoadSelectedLevel()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(selectedSceneName);
    }
}

