using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerMovementModel model;
    private PlayerMovementView view;
    private SoundManager soundManager;

    private void Awake()
    {
        model = GetComponent<PlayerMovementModel>();
        view = GetComponent<PlayerMovementView>();
    }

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    private void FixedUpdate()
    {
        // Handle movement
        model.Body.velocity = new Vector2(model.HorizontalInput * model.MoveSpeed, model.Body.velocity.y);

        if (model.JumpRequested)
        {
            if (model.IsGrounded()) Jump();
            else if (model.OnWall()) WallJump();

            model.JumpRequested = false;
        }
    }

    // Button methods
    public void StartMoveLeft() => model.HorizontalInput = -1f;
    public void StopMove() => model.HorizontalInput = 0f;
    public void StartMoveRight() => model.HorizontalInput = 1f;
    public void JumpRequest() => model.JumpRequested = true;

    private void Jump()
    {
        model.Body.velocity = new Vector2(model.Body.velocity.x, model.JumpForce);
        view.TriggerJumpAnimation();
        soundManager.PlaySound(model.JumpSound);
    }

    private void WallJump()
    {
        // Flip direction based on which wall you're touching
        float wallDirection = -Mathf.Sign(transform.localScale.x);

        model.Body.velocity = new Vector2(wallDirection * model.WallJumpX, model.WallJumpY);
        view.TriggerJumpAnimation();
        soundManager.PlaySound(model.JumpSound);
    }

    // Public interface method for ButtonPlayerAttack
    public bool CanAttack()
    {
        return model.CanAttack();
    }
} 