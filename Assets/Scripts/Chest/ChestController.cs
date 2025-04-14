using UnityEngine;

public class ChestController : MonoBehaviour
{
    private ChestModel model;

    private void Awake()
    {
        model = GetComponent<ChestModel>();
    }

    private void Start()
    {
        if (model != null)
        {
            model.OnChestOpened += HandleChestOpened;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && model != null && !model.IsOpen)
        {
            model.OpenChest();
        }
    }

    private void HandleChestOpened()
    {
        if (model.IsEnemy)
        {
            // Deduct score through the ScoreHandler service
            ScoreHandler.Instance.DeductPoints(1);
            
            // Deactivate chest after enemy is spawned
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (model != null)
        {
            model.OnChestOpened -= HandleChestOpened;
        }
    }
} 