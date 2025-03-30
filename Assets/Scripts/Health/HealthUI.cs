using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts; // Assign the 3 heart images in the inspector
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Health playerHealth;

    private int lastHealth;


    private void Start()
    {
        lastHealth = playerHealth.GetCurrentHealth();
        UpdateHealthUI(lastHealth);
        StartCoroutine(LoopEmptyHeartAnimation());

        StartCoroutine(HealthCheckRoutine());
    }


    private IEnumerator HealthCheckRoutine()
    {
        while (true)
        {
            int currentHealth = playerHealth.GetCurrentHealth();
            if (currentHealth != lastHealth)
            {
                UpdateHealthUI(currentHealth);
                lastHealth = currentHealth;
            }
            yield return new WaitForSeconds(0.1f); // Check every 0.1s
        }
    }
    private void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;  // Show full heart
            }
            else
            {
                hearts[i].sprite = emptyHeart;
                StartCoroutine(ShakeHeart(hearts[i])); // Show empty heart
            }
        }
    }

    private IEnumerator ShakeHeart(Image heart)
    {
        Vector3 originalPos = heart.transform.localPosition;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float xOffset = Random.Range(-2f, 2f);
            float yOffset = Random.Range(-2f, 2f);
            heart.transform.localPosition = originalPos + new Vector3(xOffset, yOffset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        heart.transform.localPosition = originalPos; // Reset position
    }

    private IEnumerator LoopEmptyHeartAnimation()
    {
        while (true)
        {
            foreach (Image heart in hearts)
            {
                if (heart.sprite == emptyHeart)
                {
                    Vector3 originalPos = heart.transform.localPosition;
                    heart.transform.localPosition = originalPos + new Vector3(0, 5f, 0);
                }
            }
            yield return new WaitForSeconds(0.6f);

            foreach (Image heart in hearts)
            {
                if (heart.sprite == emptyHeart)
                {
                    Vector3 originalPos = heart.transform.localPosition;
                    heart.transform.localPosition = originalPos + new Vector3(0, -5f, 0);
                }
            }
            yield return new WaitForSeconds(0.6f);
        }
    }
}
