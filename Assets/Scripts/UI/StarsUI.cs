using UnityEngine;
using UnityEngine.UI;

public class StarsUI : MonoBehaviour
{
    [SerializeField] private Image[] stars;          
    [SerializeField] private Sprite filledStar;
    [SerializeField] private Sprite emptyStar;          

    private void Start()
    {
        ShowStars();
    }

    public void ShowStars()
    {
        int starsEarned = ScoreHandler.Instance.GetCurrentScore();

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = (i < starsEarned) ? filledStar : emptyStar;
        }
    }
}
