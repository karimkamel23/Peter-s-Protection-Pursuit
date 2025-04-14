using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionView : MonoBehaviour
{

    [Header("Question UI")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;

    [Header("Question System Parameters")]
    [SerializeField] private Color correctColor, wrongColor, defaultColor;
    [SerializeField] private int penaltyAmount = 1;

    private QuestionModel currentModel;
    private UIManager uIManager;
    
    private void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
    }

    private void Awake()
    {
        // Hide question panel initially
        questionPanel.SetActive(false);
    }

    private void OnEnable()
    {
        // Find and register for all QuestionModel events in the scene
        QuestionModel[] models = FindObjectsOfType<QuestionModel>();
        foreach (var model in models)
        {
            SubscribeToModel(model);
        }
    }
    
    private void OnDisable()
    {
        // Unsubscribe from all QuestionModel events
        QuestionModel[] models = FindObjectsOfType<QuestionModel>();
        foreach (var model in models)
        {
            UnsubscribeFromModel(model);
        }
    }
    
    private void SubscribeToModel(QuestionModel model)
    {
        if (model != null)
        {
            model.OnQuestionTriggered += () => DisplayQuestion(model);
            model.OnQuestionClosed += HideQuestion;
        }
    }
    
    private void UnsubscribeFromModel(QuestionModel model)
    {
        if (model != null)
        {
            model.OnQuestionTriggered -= () => DisplayQuestion(model);
            model.OnQuestionClosed -= HideQuestion;
        }
    }

    private void DisplayQuestion(QuestionModel model)
    {
        currentModel = model;
        
        if (currentModel == null) return;

        uIManager.ToggleHUD(false);
        questionPanel.SetActive(true);
        questionText.text = currentModel.QuestionText;

        // Set up answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            string answer = (i < currentModel.Answers.Length) ? currentModel.Answers[i] : "";
            
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answer;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            SetButtonColor(i, defaultColor);
            answerButtons[i].interactable = true;
        }
    }

    private void OnAnswerSelected(int index)
    {
        if (currentModel == null) return;
        
        bool isCorrect = currentModel.IsCorrectAnswer(index);

        if (isCorrect)
        {
            SetButtonColor(index, correctColor);
        }
        else
        {
            SetButtonColor(index, wrongColor);
            SetButtonColor(currentModel.CorrectAnswerIndex, correctColor);
            ScoreHandler.Instance.DeductPoints(penaltyAmount);
        }

        // Disable all buttons after selection
        foreach (var button in answerButtons)
        {
            button.interactable = false;
        }

        // Wait 2 seconds then close question
        Invoke(nameof(CloseQuestion), 2f);
    }

    private void CloseQuestion()
    {
        if (currentModel != null)
        {
            currentModel.CloseQuestion();
        }
    }

    private void HideQuestion()
    {
        uIManager.ToggleHUD(true);
        questionPanel.SetActive(false);
        currentModel = null;
    }

    private void SetButtonColor(int index, Color color)
    {
        if (index < 0 || index >= answerButtons.Length) return;
        
        Image btnImage = answerButtons[index].GetComponent<Image>();
        if (btnImage != null)
            btnImage.color = color;
    }
} 
