using UnityEngine;

public class ChestView : MonoBehaviour
{
    [SerializeField] private Sprite openChestSprite;
    
    private SpriteRenderer spriteRenderer;
    private ChestModel model;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        model = GetComponent<ChestModel>();
    }

    private void Start()
    {
        if (model != null)
        {
            model.OnChestOpened += UpdateChestVisuals;
        }
        
    }

    private void UpdateChestVisuals()
    {
        if (spriteRenderer != null && openChestSprite != null)
        {
            spriteRenderer.sprite = openChestSprite;
        }
    }

    private void OnDestroy()
    {
        if (model != null)
        {
            model.OnChestOpened -= UpdateChestVisuals;
        }
    }
} 