using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel model;
    private PlayerView view;
    private SoundManager soundManager;

    private void Awake()
    {
        model = GetComponent<PlayerModel>();
        view = GetComponent<PlayerView>();
    }

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    private void Update()
    {
        // Don't process updates if game over
        if (model.IsGameOver) return;

        // Update cooldown timer
        model.CooldownTimer += Time.deltaTime;

        // Handle attack
        if (model.AttackRequested && model.CooldownTimer > model.AttackCooldown && model.CanAttack())
        {
            Attack();
            model.AttackRequested = false;
        }
    }

    private void FixedUpdate()
    {
        // Don't process physics if game over
        if (model.IsGameOver) return;

        // Handle movement
        model.Body.velocity = new Vector2(model.HorizontalInput * model.MoveSpeed, model.Body.velocity.y);

        if (model.JumpRequested)
        {
            if (model.IsGrounded()) Jump();
            else if (model.OnWall()) WallJump();

            model.JumpRequested = false;
        }
    }

    // Movement button methods
    public void StartMoveLeft() => model.HorizontalInput = -1f;
    public void StopMove() => model.HorizontalInput = 0f;
    public void StartMoveRight() => model.HorizontalInput = 1f;
    public void JumpRequest() => model.JumpRequested = true;

    // Attack button method
    public void AttackRequest() => model.AttackRequested = true;

    // Game over method
    public void PlayerGameOver()
    {
        model.TriggerGameOver();
    }

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

    private void Attack()
    {
        soundManager.PlaySound(model.ProjectileSound);
        view.TriggerAttackAnimation();
        model.CooldownTimer = 0;

        int projectileIndex = model.FindAvailableProjectile();
        
        model.Projectiles[projectileIndex].transform.position = model.FirePoint.position;
        model.Projectiles[projectileIndex].GetComponent<ProjectileController>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    // Public interface method for external access
    public bool CanAttack()
    {
        return model.CanAttack();
    }
} 