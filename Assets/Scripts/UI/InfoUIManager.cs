using TMPro;
using UnityEngine;

public class InfoUIManager : MonoBehaviour
{
    public static InfoUIManager Instance;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private float showTime = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void ShowInfo(string info)
    {
        infoPanel.SetActive(true);
        infoText.text = info;
        Invoke(nameof(CloseInfo), showTime);

    }

    private void CloseInfo()
    {
        infoPanel.SetActive(false);
    }
}
