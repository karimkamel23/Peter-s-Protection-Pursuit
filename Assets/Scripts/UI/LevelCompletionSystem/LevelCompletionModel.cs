using UnityEngine;
using System;

public class LevelCompletionModel : MonoBehaviour
{
    private bool isLevelCompleted = false;
    
    // Event that will notify when level is completed
    public event Action OnLevelCompleted;
    
    public bool IsLevelCompleted => isLevelCompleted;
    
    public void CompleteLevel()
    {
        if (!isLevelCompleted)
        {
            isLevelCompleted = true;
            OnLevelCompleted?.Invoke();
        }
    }
} 