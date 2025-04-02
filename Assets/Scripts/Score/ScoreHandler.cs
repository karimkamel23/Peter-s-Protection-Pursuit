using UnityEngine;

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

    public void SaveScoreForLevel(string levelKey)
    {
        //TBD
    }

    public static int GetSavedScore(string levelKey)
    {
        //TBD
        return 0;
    }
}
