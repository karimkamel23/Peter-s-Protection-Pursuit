using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionUIManager : MonoBehaviour
{
    public static QuestionUIManager Instance;

    [Header("UI Manager")]
    [SerializeField] private UIManager uIManager;

    [Header("Question UI")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;

    [Header("Question System Parameters")]
    [SerializeField] private Color correctColor, wrongColor, defaultColor;
    [SerializeField] private int penaltyAmount = 10;

    private int correctAnswerIndex;
    private Door doorToOpen;

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

        // Reset button colors
        foreach (var btn in answerButtons)
            btn.onClick.AddListener(() => { }); // Assign empty first
    }

    public void ShowQuestion(QuestionData data)
    {
        uIManager.ToggleHUD(false);
        questionPanel.SetActive(true);
        questionText.text = data.questionText;
        correctAnswerIndex = data.correctAnswerIndex;
        doorToOpen = data.doorToOpen;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = data.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            SetButtonColor(i, defaultColor);
        }
    }

    private void OnAnswerSelected(int index)
    {
        if (index == correctAnswerIndex)
        {
            SetButtonColor(index, correctColor);
        }
        else
        {
            SetButtonColor(index, wrongColor);
            SetButtonColor(correctAnswerIndex, correctColor);
            ScoreHandler.Instance.DeductPoints(penaltyAmount);
        }

        // Wait 2 seconds then hide question and re-enable player
        Invoke(nameof(CloseQuestion), 2f);
    }

    private void CloseQuestion()
    {
        uIManager.ToggleHUD(true);
        questionPanel.SetActive(false);
        doorToOpen.OpenDoor();
    }

    private void SetButtonColor(int index, Color color)
    {
        Image btnImage = answerButtons[index].GetComponent<Image>();
        if (btnImage != null)
            btnImage.color = color;
    }

}
