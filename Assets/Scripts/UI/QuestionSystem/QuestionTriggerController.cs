using UnityEngine;
using System.Collections;

public class QuestionTriggerController : MonoBehaviour
{

    [Header("Trigger Settings")]
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private AudioClip buttonPressSound;
    [SerializeField] private float questionDelay = 0.5f;

    
    private bool triggered = false;
    private SpriteRenderer spriteRenderer;
    private QuestionModel questionModel;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        questionModel = GetComponent<QuestionModel>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            spriteRenderer.sprite = pressedSprite;
            SoundManager.instance.PlaySound(buttonPressSound);
            StartCoroutine(TriggerQuestionWithDelay());
        }
    }

    private IEnumerator TriggerQuestionWithDelay()
    {
        yield return new WaitForSeconds(questionDelay);
        
        if (questionModel == null)
        {
            Debug.LogError("Question Model is not assigned in " + gameObject.name);
            yield break;
        }
        
        // Only trigger the model directly - view will listen to model event
        questionModel.TriggerQuestion();
    }
    
    // Editor validation
    private void OnValidate()
    {
        if (questionModel == null)
        {
            Debug.LogWarning("Question Model not assigned in " + gameObject.name);
        }
    }
} 