using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HealthUIView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Animation Settings")]
    [SerializeField] private float heartShakeDuration = 0.5f;
    [SerializeField] private float emptyHeartBobAmount = 5f;
    [SerializeField] private float emptyHeartBobFrequency = 0.6f;

    private HealthModel targetHealthModel;

    private void Start()
    {
        FindPlayerHealthModel();
        StartCoroutine(LoopEmptyHeartAnimation());
    }

    private void FindPlayerHealthModel()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObject != null)
        {
            HealthModel healthModel = playerObject.GetComponent<HealthModel>();
            
            if (healthModel != null)
            {
                targetHealthModel = healthModel;
                SubscribeToHealthModel();
                return;
            }
        }

        // Try alternative methods if the tag approach fails
        PlayerModel playerModel = FindObjectOfType<PlayerModel>();
        if (playerModel != null)
        {
            HealthModel healthModel = playerModel.GetComponent<HealthModel>();
            if (healthModel != null)
            {
                targetHealthModel = healthModel;
                SubscribeToHealthModel();
                return;
            }
        }

        // Final attempt with PlayerController
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            HealthModel healthModel = playerController.GetComponent<HealthModel>();
            if (healthModel != null)
            {
                targetHealthModel = healthModel;
                SubscribeToHealthModel();
                return;
            }
        }
    }

    private void SubscribeToHealthModel()
    {
        targetHealthModel.OnHealthChanged += UpdateHealthUI;
        
        // Initial update
        UpdateHealthUI(targetHealthModel.CurrentHealth, targetHealthModel.MaxHealth);
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (hearts == null || hearts.Length == 0)
        {
            return;
        }
        
        // Update heart sprites
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
                continue;
            }
            
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                if (hearts[i].sprite != emptyHeart)
                {
                    hearts[i].sprite = emptyHeart;
                    StartCoroutine(ShakeHeart(hearts[i]));
                }
            }
        }
    }

    private IEnumerator ShakeHeart(Image heart)
    {
        Vector3 originalPos = heart.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < heartShakeDuration)
        {
            float xOffset = UnityEngine.Random.Range(-2f, 2f);
            float yOffset = UnityEngine.Random.Range(-2f, 2f);
            heart.transform.localPosition = originalPos + new Vector3(xOffset, yOffset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        heart.transform.localPosition = originalPos; // Reset position
    }

    private IEnumerator LoopEmptyHeartAnimation()
    {
        // Check if hearts array is valid
        if (hearts == null || hearts.Length == 0)
        {
            yield break;
        }
        
        while (true)
        {
            // Bob empty hearts up
            foreach (Image heart in hearts)
            {
                if (heart == null) continue;
                
                if (heart.sprite == emptyHeart)
                {
                    Vector3 originalPos = heart.rectTransform.anchoredPosition;
                    heart.rectTransform.anchoredPosition = new Vector3(originalPos.x, originalPos.y + emptyHeartBobAmount, originalPos.z);
                }
            }
            yield return new WaitForSeconds(emptyHeartBobFrequency);

            // Bob empty hearts down
            foreach (Image heart in hearts)
            {
                if (heart == null) continue;
                
                if (heart.sprite == emptyHeart)
                {
                    Vector3 originalPos = heart.rectTransform.anchoredPosition;
                    heart.rectTransform.anchoredPosition = new Vector3(originalPos.x, originalPos.y - emptyHeartBobAmount, originalPos.z);
                }
            }
            yield return new WaitForSeconds(emptyHeartBobFrequency);
        }
    }

    private void OnDestroy()
    {
        // Clean up subscriptions
        if (targetHealthModel != null)
        {
            targetHealthModel.OnHealthChanged -= UpdateHealthUI;
        }
    }
} 