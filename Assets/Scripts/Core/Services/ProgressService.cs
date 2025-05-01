using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressService
{
    private static ProgressService _instance;
    public static ProgressService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ProgressService();
            }
            return _instance;
        }
    }

    // PlayerPrefs key format
    private const string PROGRESS_KEY_FORMAT = "progress_level_{0}";

    // Save progress
    public IEnumerator SaveProgress(LevelProgress progress, Action onSuccess = null, Action<string> onError = null)
    {
        
        // Save progress locally using PlayerPrefs
        SaveProgressLocally(progress);
        
        // Sync with server if online
        if (NetworkService.Instance.IsInternetAvailable())
        {
            yield return NetworkService.Instance.Post<SaveProgressResponse>("/save-progress", progress, 
                response => {
                    onSuccess?.Invoke();
                },
                errorMessage => {
                    onError?.Invoke(errorMessage);
                });
        }
        else
        {
            onSuccess?.Invoke();
        }
    }

    // Save progress locally to PlayerPrefs
    private void SaveProgressLocally(LevelProgress progress)
    {
        string key = string.Format(PROGRESS_KEY_FORMAT, progress.level_number);
        
        // Check if progress exists for this level
        if (PlayerPrefs.HasKey(key))
        {
            int currentStars = PlayerPrefs.GetInt(key);
            
            // Only update if new stars are greater
            if (progress.stars > currentStars)
            {
                PlayerPrefs.SetInt(key, progress.stars);
                PlayerPrefs.Save();
            }
            else
            {
            }
        }
        else
        {
            // No previous progress, save new
            PlayerPrefs.SetInt(key, progress.stars);
            PlayerPrefs.Save();
        }
    }

    // Sync offline progress with server
    public IEnumerator SyncOfflineProgress(Action onComplete = null)
    {
        if (!NetworkService.Instance.IsInternetAvailable() || !AuthService.Instance.IsLoggedIn())
        {
            onComplete?.Invoke();
            yield break;
        }

        User currentUser = AuthService.Instance.GetCurrentUser();
        
        List<LevelProgress> localProgress = GetAllLocalProgress();
        bool syncSuccess = true;
        
        foreach (LevelProgress progress in localProgress)
        {
            bool requestComplete = false;
            bool requestSuccess = false;
            
            yield return NetworkService.Instance.Post<SaveProgressResponse>("/save-progress", progress, 
                response => {
                    requestComplete = true;
                    requestSuccess = true;
                },
                errorMessage => {
                    requestComplete = true;
                    requestSuccess = false;
                    syncSuccess = false;
                });
            
            // Wait for the request to complete
            while (!requestComplete)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        if (!syncSuccess)
        {
            onComplete?.Invoke();
            yield break;
        }
        
        bool pullComplete = false;
        bool pullSuccess = false;
        
        yield return NetworkService.Instance.Get<ProgressResponseWrapper>($"/progress/{currentUser.id}", 
            serverProgressWrapper => {
                if (serverProgressWrapper != null && serverProgressWrapper.items != null)
                {
                    UpdateLocalProgressFromServer(serverProgressWrapper.items);
                    pullSuccess = true;
                }
                pullComplete = true;
            },
            errorMessage => {
                pullComplete = true;
            });
        
        while (!pullComplete)
        {
            yield return null;
        }
        
        onComplete?.Invoke();
    }

    // Update local PlayerPrefs with server progress data
    private void UpdateLocalProgressFromServer(List<ProgressResponse> serverProgress)
    {
        if (serverProgress == null || serverProgress.Count == 0)
        {
            return;
        }
        
        
        foreach (var progress in serverProgress)
        {
            string key = string.Format(PROGRESS_KEY_FORMAT, progress.level_number);
            int currentStars = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : 0;
            
            // Always update with server data
            PlayerPrefs.SetInt(key, progress.stars);
        }
        
        // Save all changes
        PlayerPrefs.Save();
    }

    // Get all local progress
    public List<LevelProgress> GetAllLocalProgress()
    {
        List<LevelProgress> progressList = new List<LevelProgress>();
        
        if (!AuthService.Instance.IsLoggedIn())
        {
            return progressList;
        }
        
        User currentUser = AuthService.Instance.GetCurrentUser();
        
        for (int levelNumber = 1; levelNumber <= 10; levelNumber++)
        {
            string key = string.Format(PROGRESS_KEY_FORMAT, levelNumber);
            
            if (PlayerPrefs.HasKey(key))
            {
                int stars = PlayerPrefs.GetInt(key);
                
                LevelProgress progress = new LevelProgress(
                    currentUser.id,
                    levelNumber,
                    stars,
                    true // completed is always true if we have stars
                );
                
                progressList.Add(progress);
            }
        }
        
        return progressList;
    }

    // Get progress for a specific level
    public LevelProgress GetLevelProgress(int levelNumber)
    {
        if (!AuthService.Instance.IsLoggedIn())
        {
            return null;
        }
        
        User currentUser = AuthService.Instance.GetCurrentUser();
        string key = string.Format(PROGRESS_KEY_FORMAT, levelNumber);
        
        if (PlayerPrefs.HasKey(key))
        {
            int stars = PlayerPrefs.GetInt(key);
            
            return new LevelProgress(
                currentUser.id,
                levelNumber,
                stars,
                true
            );
        }
        
        // No progress found for this level
        return null;
    }

    // Check if level is unlocked
    public bool IsLevelUnlocked(int levelNumber)
    {
        // Level 1 is always unlocked
        if (levelNumber == 1) return true;
        
        // Previous level must be completed
        LevelProgress previousLevelProgress = GetLevelProgress(levelNumber - 1);
        return previousLevelProgress != null;
    }

    // Get star count for a level (0 if not completed)
    public int GetStarsForLevel(int levelNumber)
    {
        string key = string.Format(PROGRESS_KEY_FORMAT, levelNumber);
        return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : 0;
    }

    // Clear all progress from PlayerPrefs
    public void ClearLocalProgress()
    {
        for (int levelNumber = 1; levelNumber <= 100; levelNumber++)
        {
            string key = string.Format(PROGRESS_KEY_FORMAT, levelNumber);
            
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
        
        PlayerPrefs.Save();
    }

    [Serializable]
    private class SaveProgressResponse
    {
        public string message;
    }
    
    [Serializable]
    private class ProgressResponse
    {
        public int level_number;
        public int stars;
        public int completed;
    }

    [Serializable]
    private class ProgressResponseWrapper
    {
        public List<ProgressResponse> items;
    }
} 