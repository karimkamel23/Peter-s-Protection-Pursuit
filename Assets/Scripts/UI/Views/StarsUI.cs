using UnityEngine;
using UnityEngine.UI;

public class StarsUI : MonoBehaviour
{
    [SerializeField] private Image[] stars;          
    [SerializeField] private Sprite filledStar;
    [SerializeField] private Sprite emptyStar; 

    [Header("For Level Select")]       
    [SerializeField] private int levelNumber;

    private int starsEarned;

    private void Start()
    {
        ShowStars();
    }

    public void ShowStars()
    {
        if (levelNumber == null || levelNumber == 0) starsEarned = ScoreHandler.Instance.GetCurrentScore();
        else starsEarned = ScoreHandler.GetSavedScore(levelNumber);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = (i < starsEarned) ? filledStar : emptyStar;
        }
    }
}
