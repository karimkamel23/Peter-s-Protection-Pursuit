using UnityEngine;
using System.Collections;

public class StartupController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private bool autoLoadMainMenu = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Load cached session from PlayerPrefs
        User cachedUser = AuthService.Instance.LoadCachedSession();
        
        // Decide where to start
        if (cachedUser != null)
        {
            StartCoroutine(LoadMainMenuWithSync());
        }
        else if (autoLoadMainMenu)
        {
            uiManager.MainMenu();
        }
        else
        {
            uiManager.ShowLoginUI();
        }
    }
    
    private IEnumerator LoadMainMenuWithSync()
    {
        uiManager.MainMenu();
        
        // Sync offline progress if we have internet
        if (NetworkService.Instance.IsInternetAvailable())
        {
            yield return StartCoroutine(ProgressService.Instance.SyncOfflineProgress());
        }
    }
} 