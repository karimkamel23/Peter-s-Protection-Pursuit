using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private Vector3 mainScale;
    private float horizontalInput;
    private float wallJumpCooldown;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainScale = transform.localScale;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        //body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip character
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);
        if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);

        // Animation states
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y >0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 1;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }
        
    }

    private void FixedUpdate()
    {
        anim.SetBool("Grounded", isGrounded());
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.01f,
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
            0.01f,
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
        // Unused for now
    }
}
