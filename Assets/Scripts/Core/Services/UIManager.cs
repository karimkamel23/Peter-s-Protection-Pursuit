using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;


public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("Other Screens")]
    [SerializeField] private GameObject[] HUDs;

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneLoader.Instance.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        if (HasNextLevel())
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneLoader.Instance.LoadScene(nextIndex);
        }
    }

    // Helper method to check if a next level exists
    private bool HasNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        return (currentIndex + 1 < totalScenes);
    }

    public void ToggleHUD(bool status)
    {
        foreach (GameObject screen in HUDs)
        {
            if (screen != null) screen.SetActive(status);
        }
    }

    // Audio control methods
    public void SetMusicVolume(float value)
    {
        SoundManager.instance.ChangeMusicVolume(value);
    }

    public void SetSoundVolume(float value)
    {
        SoundManager.instance.ChangeSoundVolume(value);
    }

    public float GetMusicVolume()
    {
        return SoundManager.instance.GetCurrentMusicVolume();
    }

    public float GetSoundVolume()
    {
        return SoundManager.instance.GetCurrentSoundVolume();
    }
}

