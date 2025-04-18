using UnityEngine;
using System.Collections;

public class ScoreHandler : MonoBehaviour
{
    public static ScoreHandler Instance;

    [SerializeField] private int maxScore = 5;
    private int CurrentScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ResetScore();
    }

    public void DeductPoints(int amount)
    {
        CurrentScore = Mathf.Max(0, CurrentScore - amount);
    }

    public void ResetScore()
    {
        CurrentScore = maxScore;
    }

    public int GetCurrentScore()
    {
        float percent = (float)CurrentScore / maxScore;
        int stars = Mathf.Clamp(Mathf.FloorToInt(percent * 3), 0, 3);
        return stars;
    }

    public void SaveScoreForLevel(int levelNumber)
    {
        int stars = GetCurrentScore();
        User currentUser = AuthService.Instance.GetCurrentUser();
        
        if (currentUser != null)
        {
            LevelProgress progress = new LevelProgress(
                currentUser.id,
                levelNumber,
                stars,
                true
            );
            
            StartCoroutine(ProgressService.Instance.SaveProgress(progress));
        }
    }

    public static int GetSavedScore(int levelNumber)
    {
        return ProgressService.Instance.GetStarsForLevel(levelNumber);
    }
}
