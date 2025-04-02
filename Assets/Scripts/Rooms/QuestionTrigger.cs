using UnityEngine;
using System.Collections;

public class QuestionTrigger : MonoBehaviour
{
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private QuestionData question;
    [SerializeField] private AudioClip buttonPressSound;

    private bool triggered = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            spriteRenderer.sprite = pressedSprite;
            SoundManager.instance.PlaySound(buttonPressSound);
            StartCoroutine(ShowQuestionWithDelay());
        }
    }

    private IEnumerator ShowQuestionWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        QuestionUIManager.Instance.ShowQuestion(question);
    }
}
