using TMPro;
using UnityEngine;
using System.Collections;

public class InfoView : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private float showTime = 5f;
    
    private Coroutine hideCoroutine;
    
    private void Start()
    {
        // Make sure the panel is hidden initially
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
        
        // Find all info models in the scene and subscribe to their events
        InfoModel[] infoModels = FindObjectsOfType<InfoModel>(true);
        foreach (InfoModel model in infoModels)
        {
            model.OnInfoTriggered += ShowInfo;
        }
    }
    
    public void ShowInfo(string info)
    {
        // Update text and show panel
        if (infoPanel != null && infoText != null)
        {
            infoText.text = info;
            infoPanel.SetActive(true);
            
            // Cancel existing hide coroutine if any
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }
            
            // Start new hide coroutine
            hideCoroutine = StartCoroutine(HideAfterDelay());
        }
    }
    
    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showTime);
        
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
        
        hideCoroutine = null;
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from all info models
        InfoModel[] infoModels = FindObjectsOfType<InfoModel>(true);
        foreach (InfoModel model in infoModels)
        {
            model.OnInfoTriggered -= ShowInfo;
        }
    }
} 