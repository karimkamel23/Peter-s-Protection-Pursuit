using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;


public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("Other Screens")]
    [SerializeField] private GameObject[] HUDs;

    [Header("Login & Register UI")]
    
    // Login fields
    [SerializeField] private GameObject loginFields; // LOGIN panel 
    [SerializeField] private TMP_InputField loginUsernameInput;
    [SerializeField] private TMP_InputField loginPasswordInput;
    [SerializeField] private TMP_Text loginErrorText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button goToRegisterButton;
    
    // Register fields
    [SerializeField] private GameObject registerFields; // RegisterPanel
    [SerializeField] private TMP_InputField registerUsernameInput;
    [SerializeField] private TMP_InputField registerPasswordInput;
    [SerializeField] private TMP_InputField confirmPasswordInput;
    [SerializeField] private TMP_InputField registerEmailInput;
    [SerializeField] private TMP_Text registerErrorText;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button goToLoginButton;

    [Header("User Info")]
    [SerializeField] private GameObject userInfoPanel;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private Button logoutButton;

    [Header("Scene Names")]
    [SerializeField] private string loginSceneName = "LoginAndSignUp";
    [SerializeField] private string mainMenuSceneName = "_MainMenu";

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Assign button listeners if login UI is present
        if (loginButton != null && registerButton != null)
        {
            loginButton.onClick.AddListener(OnLoginButtonPressed);
            registerButton.onClick.AddListener(OnRegisterButtonPressed);
            goToRegisterButton.onClick.AddListener(() => SwitchLoginRegisterPanels(true));
            goToLoginButton.onClick.AddListener(() => SwitchLoginRegisterPanels(false));
        }

        if (logoutButton != null)
        {
            logoutButton.onClick.AddListener(OnLogoutButtonPressed);
        }

        // Initialize user info panel if available
        if (userInfoPanel != null && usernameText != null)
        {
            User currentUser = AuthService.Instance.GetCurrentUser();
            if (currentUser != null)
            {
                usernameText.text = currentUser.username;
                userInfoPanel.SetActive(true);
            }
            else
            {
                userInfoPanel.SetActive(false);
            }
        }

        // Initially show login panel
        SwitchLoginRegisterPanels(false);
    }

    // Show login UI
    public void ShowLoginUI()
    {
        SceneLoader.Instance.LoadScene(loginSceneName);
    }

    // Switch between login and register views
    private void SwitchLoginRegisterPanels(bool showRegister)
    {
        if (registerFields != null && loginFields != null)
        {
            registerFields.SetActive(showRegister);
            loginFields.SetActive(!showRegister);
            ClearInputFields();
            HideError();
        }
    }

    // Login button handler
    public void OnLoginButtonPressed()
    {
        if (string.IsNullOrEmpty(loginUsernameInput.text) || string.IsNullOrEmpty(loginPasswordInput.text))
        {
            DisplayError("Please enter username and password");
            return;
        }

        HideError();
        loginButton.interactable = false;

        StartCoroutine(AuthService.Instance.Login(
            loginUsernameInput.text, 
            loginPasswordInput.text, 
            user => {
                // Login success
                loginButton.interactable = true;
                ClearInputFields();
                
                // Sync progress first, then show main menu
                StartCoroutine(SyncProgressAndShowMainMenu());
            },
            errorMessage => {
                // Login failed
                loginButton.interactable = true;
                DisplayError(errorMessage);
            }
        ));
    }
    
    private IEnumerator SyncProgressAndShowMainMenu()
    {
        // First sync progress
        yield return StartCoroutine(ProgressService.Instance.SyncOfflineProgress());
        
        // Then show main menu
        MainMenu();
    }

    // Register button handler
    public void OnRegisterButtonPressed()
    {
        if (string.IsNullOrEmpty(registerUsernameInput.text) || 
            string.IsNullOrEmpty(registerPasswordInput.text) || 
            string.IsNullOrEmpty(registerEmailInput.text))
        {
            DisplayError("Please fill all fields");
            return;
        }
        
        // Validate password strength
        string passwordValidation = ValidatePassword(registerPasswordInput.text);
        if (passwordValidation != "valid")
        {
            DisplayError(passwordValidation);
            return;
        }
        
        // Check if passwords match
        if (registerPasswordInput.text != confirmPasswordInput.text)
        {
            DisplayError("Passwords do not match");
            return;
        }
        
        // Simple email validation
        if (!IsValidEmail(registerEmailInput.text))
        {
            DisplayError("Please enter a valid email");
            return;
        }

        HideError();
        registerButton.interactable = false;

        StartCoroutine(AuthService.Instance.Register(
            registerUsernameInput.text, 
            registerPasswordInput.text, 
            registerEmailInput.text,
            user => {
                // Registration success
                registerButton.interactable = true;
                ClearInputFields();
                MainMenu();
            },
            errorMessage => {
                // Registration failed
                registerButton.interactable = true;
                DisplayError(errorMessage);
            }
        ));
    }
    
    // Password validation function
    private string ValidatePassword(string password)
    {
        // Check length
        if (password.Length < 8)
            return "Password must be at least 8 characters";
            
        if (password.Length > 20)
            return "Password cannot exceed 20 characters";
            
        // Check for lowercase letter
        bool hasLower = false;
        // Check for uppercase letter
        bool hasUpper = false;
        // Check for digit
        bool hasDigit = false;
        // Check for special character
        bool hasSpecial = false;
        
        foreach (char c in password)
        {
            if (char.IsLower(c))
                hasLower = true;
            else if (char.IsUpper(c))
                hasUpper = true;
            else if (char.IsDigit(c))
                hasDigit = true;
            else if (!char.IsLetterOrDigit(c))
                hasSpecial = true;
        }
        
        if (!hasLower)
            return "Password must contain a lowercase letter";
            
        if (!hasUpper)
            return "Password must contain an uppercase letter";
            
        if (!hasDigit)
            return "Password must contain a number";
            
        if (!hasSpecial)
            return "Password must contain a special character";
            
        return "valid";
    }

    // Helper method to validate email format with simple check
    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    // Logout button handler
    public void OnLogoutButtonPressed()
    {
        AuthService.Instance.Logout();
        ShowLoginUI();
    }

    // Display error message
    public void DisplayError(string message)
    {
        // Determine which error text to use based on active panel
        if (loginFields != null && registerFields != null)
        {
            if (loginFields.activeSelf && loginErrorText != null)
            {
                loginErrorText.gameObject.SetActive(true);
                loginErrorText.text = message;
            }
            else if (registerFields.activeSelf && registerErrorText != null)
            {
                registerErrorText.gameObject.SetActive(true);
                registerErrorText.text = message;
            }
        }
    }

    // Hide error message
    public void HideError()
    {
        // Hide both error texts
        if (loginErrorText != null)
        {
            loginErrorText.gameObject.SetActive(false);
        }
        
        if (registerErrorText != null)
        {
            registerErrorText.gameObject.SetActive(false);
        }
    }

    // Clear input fields
    private void ClearInputFields()
    {
        // Clear login fields
        if (loginUsernameInput != null) loginUsernameInput.text = "";
        if (loginPasswordInput != null) loginPasswordInput.text = "";
        
        // Clear register fields
        if (registerUsernameInput != null) registerUsernameInput.text = "";
        if (registerPasswordInput != null) registerPasswordInput.text = "";
        if (registerEmailInput != null) registerEmailInput.text = "";
        if (confirmPasswordInput != null) confirmPasswordInput.text = "";
    }

    // Existing methods
    public void Restart()
    {
        Time.timeScale = 1;
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneLoader.Instance.LoadScene(mainMenuSceneName);
    }

    public void LoadNextLevel()
    {
        if (HasNextLevel())
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneLoader.Instance.LoadScene(nextIndex);
        }
    }

    // Helper method to check if a next level exists
    private bool HasNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        return (currentIndex + 1 < totalScenes);
    }

    public void ToggleHUD(bool status)
    {
        foreach (GameObject screen in HUDs)
        {
            if (screen != null) screen.SetActive(status);
        }
    }

    // Audio control methods
    public void SetMusicVolume(float value)
    {
        SoundManager.instance.ChangeMusicVolume(value);
    }

    public void SetSoundVolume(float value)
    {
        SoundManager.instance.ChangeSoundVolume(value);
    }

    public float GetMusicVolume()
    {
        return SoundManager.instance.GetCurrentMusicVolume();
    }

    public float GetSoundVolume()
    {
        return SoundManager.instance.GetCurrentSoundVolume();
    }
}

