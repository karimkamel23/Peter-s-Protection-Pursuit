using UnityEngine;
using System;

public class PlayerModel : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private AudioClip projectileSound;

    // Event for game over state
    public event Action OnPlayerGameOver;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Vector3 mainScale;

    private float horizontalInput = 0f;
    private bool jumpRequested = false;
    private bool attackRequested = false;
    private float cooldownTimer = Mathf.Infinity;
    private bool isGameOver = false;

    // Movement property accessors
    public float MoveSpeed => moveSpeed;
    public float JumpForce => jumpForce;
    public float WallJumpX => wallJumpX;
    public float WallJumpY => wallJumpY;
    public LayerMask GroundLayer => groundLayer;
    public LayerMask WallLayer => wallLayer;
    public AudioClip JumpSound => jumpSound;
    public Rigidbody2D Body => body;
    public BoxCollider2D BoxCollider => boxCollider;
    public Vector3 MainScale => mainScale;
    public float HorizontalInput { get => horizontalInput; set => horizontalInput = value; }
    public bool JumpRequested { get => jumpRequested; set => jumpRequested = value; }

    // Attack property accessors
    public float AttackCooldown => attackCooldown;
    public Transform FirePoint => firePoint;
    public GameObject[] Projectiles => projectiles;
    public AudioClip ProjectileSound => projectileSound;
    public bool AttackRequested { get => attackRequested; set => attackRequested = value; }
    public float CooldownTimer { get => cooldownTimer; set => cooldownTimer = value; }

    // Game over property
    public bool IsGameOver { get => isGameOver; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainScale = transform.localScale;
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer
        );

        return raycastHit.collider != null;
    }

    public bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer
        );

        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }

    public int FindAvailableProjectile()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy) return i;
        }

        return 0;
    }

    public void TriggerGameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            OnPlayerGameOver?.Invoke();
        }
    }
} 