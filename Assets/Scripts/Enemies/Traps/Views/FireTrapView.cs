using UnityEngine;

public class FireTrapView : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private readonly Color warningColor = Color.red;
    private readonly Color activeColor = Color.white;
    private readonly Color inactiveColor = new Color(0, 179, 255, 255); // Light blue

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetWarningState()
    {
        spriteRenderer.color = warningColor;
    }

    public void SetActiveState()
    {
        spriteRenderer.color = activeColor;
        animator.SetBool("activated", true);
    }

    public void SetInactiveState()
    {
        animator.SetBool("activated", false);
        spriteRenderer.color = inactiveColor;
    }

    public void PlayActivationSound(AudioClip soundClip)
    {
        if (soundClip != null)
        {
            SoundManager.instance.PlaySound(soundClip); 
        }
    }
} 