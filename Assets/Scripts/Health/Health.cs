using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private HealthUI healthUI;

    private int currentHealth;
    private Animator anim;
    private bool dead;

    [Header("Iframes")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        healthUI.UpdateHealthUI(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthUI.UpdateHealthUI(currentHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invurnability());
        }
        else {
            if (!dead)
            {
                anim.SetTrigger("death");
                GetComponent<PlayerMovement>().enabled = false; /////////////////////////////////
                GetComponent<ButtonPlayerMovement>().enabled = false;
                GetComponent<ButtonPlayerAttack>().enabled = false;

                dead = true;
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthUI.UpdateHealthUI(currentHealth);
    }

    private IEnumerator Invurnability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        for (int i = 0; i < numberOfFlashes; i++) { 
            spriteRenderer.color = new Color(1,0,0,0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes*2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);

    }

}
