using UnityEngine;

public class DoorView : MonoBehaviour
{
    [SerializeField] private GameObject doorCollider;
    
    private SpriteRenderer spriteRenderer;
    private DoorModel model;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        model = GetComponent<DoorModel>();
    }
    
    private void Start()
    {
        if (model != null)
        {
            model.OnDoorOpened += HandleDoorOpened;
        }
    }
    
    private void HandleDoorOpened()
    {
        // Hide door sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        
        // Disable door collider
        if (doorCollider != null)
        {
            doorCollider.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        if (model != null)
        {
            model.OnDoorOpened -= HandleDoorOpened;
        }
    }
} 