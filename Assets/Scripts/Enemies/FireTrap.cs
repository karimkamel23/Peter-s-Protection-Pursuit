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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!triggered)
                StartCoroutine(ActivateFireTrap());
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);
    }



    private IEnumerator ActivateFireTrap()
    {
        // trun sprite red as warning
        triggered = true;
        spriteRenderer.color = Color.red;

        //wait then activate trap and turn on animation
        yield return new WaitForSeconds(activationDelay);
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
