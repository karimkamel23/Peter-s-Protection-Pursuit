using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private AudioClip pauseSound;

    [Header("Audio")]
    [SerializeField] private GameObject audioScreen;
    [SerializeField] Slider soundSlider;
    [SerializeField] TextMeshProUGUI soundPercentage;
    [SerializeField] Slider musicSlider;
    [SerializeField] TextMeshProUGUI musicPercentage;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private PlayerUIAnimation playerAnimation;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
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

    private void UpdateVolume()
    {
        UpdateMusicPercentage();
        UpdateSoundPercentage();
    }
    private void UpdateMusicPercentage()
    {
        musicPercentage.text = Mathf.RoundToInt(SoundManager.instance.GetCurrentMusicVolume() * 100f) + "%";
    }

    private void UpdateSoundPercentage()
    {
        soundPercentage.text = Mathf.RoundToInt(SoundManager.instance.GetCurrentSoundVolume() * 100f) + "%";
    }

    public void OpenAudioScreen()
    {
        audioScreen.gameObject.SetActive(true);
        SetVolumeSliders();
    }

    public void CloseAudioScreen()
    {
        audioScreen.gameObject.SetActive(false);
    }


    public void GoToLevelSelect()
    {
        playerAnimation.PlayPlayerAnimation();
        mainMenuScreen.gameObject.SetActive(false);
    }
}
