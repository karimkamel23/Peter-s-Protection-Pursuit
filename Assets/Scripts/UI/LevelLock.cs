using UnityEngine;
using UnityEngine.UI;

public class LevelLock : MonoBehaviour
{
    [Header("Level Information")]
    [SerializeField] private int levelNumber;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Button levelButton;
    
    private void Start()
    {
        UpdateLockState();
    }
    
    private void OnEnable()
    {
        // Refresh lock state when object is enabled (returning to level select screen)
        UpdateLockState();
    }
    
    private void UpdateLockState()
    {
        // Determine if level should be unlocked
        bool isUnlocked = IsLevelUnlocked();
        
        // Set lock icon visibility
        if (lockIcon != null)
        {
            lockIcon.SetActive(!isUnlocked);
        }
        
        // Enable/disable button
        if (levelButton != null)
        {
            levelButton.interactable = isUnlocked;
        }
    }
    
    private bool IsLevelUnlocked()
    {
        // Use ProgressService to check if level is unlocked
        return ProgressService.Instance.IsLevelUnlocked(levelNumber);
    }
} 