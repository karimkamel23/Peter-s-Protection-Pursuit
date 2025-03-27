using UnityEngine;
//using System.Collections; ////////////////////////////////////////////

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private HealthUI healthUI;

    private int currentHealth;
    private Animator anim;
    private bool dead;


    void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthUI.UpdateHealthUI(currentHealth);
        //StartCoroutine(AutoDamage()); /////////////////////////////
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthUI.UpdateHealthUI(currentHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
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

    //// **TESTING: Lose 1 heart every 5 seconds**
    //private IEnumerator AutoDamage()
    //{
    //    while (currentHealth > 0)
    //    {
    //        yield return new WaitForSeconds(5f); // Wait for 5 seconds
    //        TakeDamage(1); // Lose 1 heart
    //        Debug.Log("Lost 1 heart! Current Health: " + currentHealth);
    //    }
    //}
}
