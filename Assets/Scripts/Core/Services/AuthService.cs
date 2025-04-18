using System;
using System.Collections;
using UnityEngine;

public class AuthService
{
    private static AuthService _instance;
    public static AuthService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AuthService();
            }
            return _instance;
        }
    }

    // PlayerPrefs keys
    private const string USER_ID_KEY = "user_id";
    private const string USERNAME_KEY = "username";
    private const string EMAIL_KEY = "email";

    private User currentUser;

    // Login user
    public IEnumerator Login(string username, string password, Action<User> onSuccess, Action<string> onError)
    {
        var loginData = new LoginRequest { username = username, password = password };

        // Send login request to server
        if (NetworkService.Instance.IsInternetAvailable())
        {
            yield return NetworkService.Instance.Post<LoginResponse>("/login", loginData, 
                response => {
                    currentUser = new User(response.id, response.username, response.email);
                    SaveUserToPlayerPrefs(currentUser);
                    onSuccess?.Invoke(currentUser);
                },
                errorMessage => {
                    onError?.Invoke(errorMessage);
                });
        }
        else
        {
            onError?.Invoke("No internet connection. Please try again later.");
        }
    }

    // Register new user
    public IEnumerator Register(string username, string password, string email, Action<User> onSuccess, Action<string> onError)
    {
        var registerData = new RegisterRequest { username = username, password = password, email = email };

        // Send registration request to server
        if (NetworkService.Instance.IsInternetAvailable())
        {
            yield return NetworkService.Instance.Post<RegisterResponse>("/register", registerData, 
                response => {
                    currentUser = new User(response.id, response.username, response.email);
                    SaveUserToPlayerPrefs(currentUser);
                    onSuccess?.Invoke(currentUser);
                },
                errorMessage => {
                    onError?.Invoke(errorMessage);
                });
        }
        else
        {
            onError?.Invoke("No internet connection. Please try again later.");
        }
    }

    // Save user to PlayerPrefs
    private void SaveUserToPlayerPrefs(User user)
    {
        PlayerPrefs.SetInt(USER_ID_KEY, user.id);
        PlayerPrefs.SetString(USERNAME_KEY, user.username);
        PlayerPrefs.SetString(EMAIL_KEY, user.email);
        PlayerPrefs.Save();
    }

    // Load cached session from PlayerPrefs
    public User LoadCachedSession()
    {
        if (PlayerPrefs.HasKey(USER_ID_KEY))
        {
            int id = PlayerPrefs.GetInt(USER_ID_KEY);
            string username = PlayerPrefs.GetString(USERNAME_KEY);
            string email = PlayerPrefs.GetString(EMAIL_KEY);
            
            currentUser = new User(id, username, email);
            return currentUser;
        }
        
        return null;
    }

    // Logout
    public void Logout()
    {
        // Clear user data from PlayerPrefs
        PlayerPrefs.DeleteKey(USER_ID_KEY);
        PlayerPrefs.DeleteKey(USERNAME_KEY);
        PlayerPrefs.DeleteKey(EMAIL_KEY);
        
        // Clear progress data
        ProgressService.Instance.ClearLocalProgress();
        
        // Save changes
        PlayerPrefs.Save();
        
        currentUser = null;
    }

    // Check if user is logged in
    public bool IsLoggedIn()
    {
        return currentUser != null || PlayerPrefs.HasKey(USER_ID_KEY);
    }

    // Get current user
    public User GetCurrentUser()
    {
        if (currentUser == null)
        {
            currentUser = LoadCachedSession();
        }
        return currentUser;
    }
    
    // Helper classes for requests and responses
    [Serializable]
    private class LoginRequest
    {
        public string username;
        public string password;
    }
    
    [Serializable]
    private class LoginResponse
    {
        public int id;
        public string username;
        public string email;
    }
    
    [Serializable]
    private class RegisterRequest
    {
        public string username;
        public string password;
        public string email;
    }
    
    [Serializable]
    private class RegisterResponse
    {
        public int id;
        public string username;
        public string email;
    }
} 