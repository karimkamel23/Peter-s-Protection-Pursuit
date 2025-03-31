using UnityEngine;

public class GameOverPlayer : MonoBehaviour
{
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    public void PlayerGameOver()
    {
        uiManager.GameOver();
    }
}
