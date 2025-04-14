using System.Collections;
using UnityEngine;

public class FireTrapController : MonoBehaviour
{
    [SerializeField] private FireTrapModel model;
    
    private FireTrapView view;
    private HealthController playerHealth;

    private void Awake()
    {
        view = GetComponent<FireTrapView>();
    }

    private void Update()
    {
        if (playerHealth != null && model.Active)
            playerHealth.TakeDamage(model.Damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<HealthController>();

            if (!model.Triggered)
                StartCoroutine(ActivateFireTrap());

            if (model.Active)
                playerHealth.TakeDamage(model.Damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerHealth = null;
    }

    private IEnumerator ActivateFireTrap()
    {
        // Set warning state
        model.Triggered = true;
        view.SetWarningState();

        // Wait for activation delay
        yield return new WaitForSeconds(model.ActivationDelay);
        
        // Play sound and activate trap
        view.PlayActivationSound(model.FiretrapSound);
        model.Active = true;
        view.SetActiveState();

        // Wait for active duration then deactivate
        yield return new WaitForSeconds(model.ActiveTime);
        model.Active = false;
        model.Triggered = false;
        view.SetInactiveState();
    }
} 