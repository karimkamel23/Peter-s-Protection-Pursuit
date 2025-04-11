using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private int damage;
    
    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool triggered;
    private bool active;

    private HealthModel playerHealth;

    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerHealth != null && active)
            playerHealth.TakeDamage(damage);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<HealthModel>();

            if (!triggered)
                StartCoroutine(ActivateFireTrap());

            if (active)
                playerHealth.TakeDamage(damage);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerHealth = null;
    }



    private IEnumerator ActivateFireTrap()
    {
        // trun sprite red as warning
        triggered = true;
        spriteRenderer.color = Color.red;

        //wait then activate trap and turn on animation
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        spriteRenderer.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        //wait then deactivate trap and turn off animation
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
        spriteRenderer.color = new Color(0, 179, 255, 255);


    }
}
