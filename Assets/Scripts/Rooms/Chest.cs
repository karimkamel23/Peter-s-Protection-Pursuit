using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Sprite openChestSprite;
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] bool isEnemy;

    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpen)
        {
            isOpen = true;
            spriteRenderer.sprite = openChestSprite;
            objectToSpawn.SetActive(true);
            if (isEnemy) { 
                gameObject.SetActive(false);
                ScoreHandler.Instance.DeductPoints(1);
            }
        }
    }
}
