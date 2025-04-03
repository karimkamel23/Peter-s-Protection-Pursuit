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
            Debug.Log("Coroutine");
            int currentHealth = playerHealth.GetCurrentHealth();
            Debug.Log("Current = "+ currentHealth + "last: "+ lastHealth);
            if (currentHealth != lastHealth)
            {
                Debug.Log("Health Changed to "+ currentHealth);
                UpdateHealthUI(currentHealth);
                lastHealth = currentHealth;
            }
            yield return new WaitForSeconds(0.1f); 
        }
    }
    private void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                Debug.Log("Showing Full");
                hearts[i].sprite = fullHeart;  // Show full heart
            }
            else
            {
                Debug.Log("Showing Empty");
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
