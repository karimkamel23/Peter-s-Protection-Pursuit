using UnityEngine;

public class EnemyProjectileView : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void PlayExplodeAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("explode");
        }else gameObject.SetActive(false);
    }

    public void PlayImpactSound(AudioClip soundClip)
    {
        if (soundClip != null)
        {
            SoundManager.instance.PlaySound(soundClip);
        }
    }
} 