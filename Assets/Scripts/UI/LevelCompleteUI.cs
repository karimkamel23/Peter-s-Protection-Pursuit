using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private GameObject nextLevelButton;

    private void Start()
    {
        if (HasNextLevel())
        {
            nextLevelButton.SetActive(true);
        }
        else
        {
            nextLevelButton.SetActive(false);
        }
    }

    private bool HasNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        return (currentIndex + 1 < totalScenes);
    }

    public void LoadNextLevel()
    {
        if (HasNextLevel())
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            LoadingScreen.Instance.LoadScene(nextIndex);
        }
    }
}
