using UnityEngine;

public class PlayerMovementView : MonoBehaviour
{
    private Animator animator;
    private Transform playerTransform;
    private PlayerMovementModel model;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTransform = transform;
        model = GetComponent<PlayerMovementModel>();
    }

    private void Update()
    {
        UpdateSpriteDirection();
        UpdateAnimations();
    }

    private void UpdateSpriteDirection()
    {
        Vector3 mainScale = model.MainScale;
        
        if (model.HorizontalInput > 0.01f)
            playerTransform.localScale = new Vector3(Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);

        if (model.HorizontalInput < -0.01f)
            playerTransform.localScale = new Vector3(-Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);
    }

    private void UpdateAnimations()
    {
        animator.SetBool("Run", model.HorizontalInput != 0);
        animator.SetBool("Grounded", model.IsGrounded());
    }

    public void TriggerJumpAnimation()
    {
        animator.SetTrigger("Jump");
    }
} 