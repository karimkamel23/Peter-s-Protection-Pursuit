using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class QuestionData
{
    public string questionText;
    public string[] answers;
    public int correctAnswerIndex;
    public DoorController doorToOpen;
}

public class QuestionModel : MonoBehaviour
{
    [SerializeField] private QuestionData questionData;
    
    // Events
    public event Action OnQuestionTriggered;
    public event Action OnQuestionClosed;
    
    // Properties
    public string QuestionText => questionData.questionText;
    public string[] Answers => questionData.answers;
    public int CorrectAnswerIndex => questionData.correctAnswerIndex;
    public DoorController DoorToOpen => questionData.doorToOpen;
    
    // Optional properties for enhanced UI
    public Sprite BackgroundImage { get; private set; }
    public string CorrectFeedback => "Correct!";
    public string IncorrectFeedback => "Try again!";
    public float DelayAfterAnswer => 2f;
    public List<string> AnswersList => new List<string>(questionData.answers);
    
    public void TriggerQuestion()
    {
        OnQuestionTriggered?.Invoke();
    }
    
    public void CloseQuestion()
    {
        OnQuestionClosed?.Invoke();
        if (DoorToOpen != null)
        {
            DoorToOpen.OpenDoor();
        }
    }
    
    public bool IsCorrectAnswer(int answerIndex)
    {
        return answerIndex == questionData.correctAnswerIndex;
    }
} 