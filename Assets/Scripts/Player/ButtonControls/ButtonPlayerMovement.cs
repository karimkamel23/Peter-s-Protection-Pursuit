using UnityEngine;

public class ButtonPlayerMovement : MonoBehaviour
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

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private Vector3 mainScale;

    private float horizontalInput = 0f;
    private bool jumpRequested = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainScale = transform.localScale;
    }

    private void Update()
    {
        
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);

        if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);

        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
    }

    private void FixedUpdate()
    {
        // Handle movement and animation
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);

        if (jumpRequested)
        {
            if (isGrounded()) Jump();
            else if (onWall()) WallJump();

            jumpRequested = false; 
        }
    }

    // Button methods
    public void StartMoveLeft() => horizontalInput = -1f;
    public void StopMove() => horizontalInput = 0f;
    public void StartMoveRight() => horizontalInput = 1f;

    public void JumpRequest() => jumpRequested = true;

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        anim.SetTrigger("Jump");
        SoundManager.instance.PlaySound(jumpSound);
    }

    private void WallJump()
    {
        // Flip direction based on which wall you're touching
        float wallDirection = -Mathf.Sign(transform.localScale.x);

        body.velocity = new Vector2(wallDirection * wallJumpX, wallJumpY);
        anim.SetTrigger("Jump");
        SoundManager.instance.PlaySound(jumpSound);
    }

    private bool isGrounded()
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

    private bool onWall()
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
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

}

