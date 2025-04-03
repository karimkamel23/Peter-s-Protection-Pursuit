using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ComicCutscene : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.2f;
    [SerializeField] private float delayBeforeStart = 1f;
    [SerializeField] private float delayAfterEnd = 1f;

    private ScrollRect scrollRect;


    private bool isScrolling = false;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        StartCoroutine(ScrollComic());
    }

    IEnumerator ScrollComic()
    {
        yield return new WaitForSeconds(delayBeforeStart);
        isScrolling = true;

        while (scrollRect.verticalNormalizedPosition > 0)
        {
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = 0;

        yield return new WaitForSeconds(delayAfterEnd);
        LoadLevelScene();
    }

    public void SkipCutscene()
    {
        StopAllCoroutines();
        LoadLevelScene();
    }

    void LoadLevelScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        LoadingScreen.Instance.LoadScene(nextIndex);
    }
}
