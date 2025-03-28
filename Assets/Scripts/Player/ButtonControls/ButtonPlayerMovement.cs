using UnityEngine;

public class ButtonPlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

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
        // Handle movement and animation
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);

        if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);

        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
    }

    private void FixedUpdate()
    {
        if (jumpRequested && (isGrounded() || onWall()))
        {
            Jump();
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

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Not used currently
    }
}

