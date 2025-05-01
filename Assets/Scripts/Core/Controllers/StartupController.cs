using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class StartupController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private bool autoLoadMainMenu = false;

    [Header("Typewriter CLI Settings")]
    [SerializeField] private TextMeshProUGUI cliText;
    [TextArea]
    [SerializeField] private string startupText;
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("Fade Out Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        cliText.text = "";
        fadeImage.color = new Color(0, 0, 0, 0); 
        StartCoroutine(TypewriterStartup());
    }

    private IEnumerator TypewriterStartup()
    {
        foreach (char c in startupText)
        {
            cliText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        StartCoroutine(BlinkingCursor());

        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(FadeOut());

        User cachedUser = AuthService.Instance.LoadCachedSession();

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

    private IEnumerator BlinkingCursor()
    {
        while (true)
        {
            if (cliText.text.EndsWith("|"))
                cliText.text = cliText.text.Substring(0, cliText.text.Length - 1);
            else
                cliText.text += "|";

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator FadeOut()
    {
        Color c = fadeImage.color;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, normalized);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 1f);
    }

    private IEnumerator LoadMainMenuWithSync()
    {
        if (NetworkService.Instance.IsInternetAvailable())
        {
            yield return StartCoroutine(ProgressService.Instance.SyncOfflineProgress(() => {
                uiManager.MainMenu();
            }));
        }
        else
        {
            uiManager.MainMenu();
        }
    }
}

