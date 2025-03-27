using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private Vector3 mainScale;
    private float horizontalInput;

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
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip character
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);
        if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(mainScale.x), mainScale.y, mainScale.z);

        // Jump (one-time press)
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded() || onWall()))
            Jump();

        // Animation states
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
    }

    private void Jump()
    {
        anim.SetTrigger("Jump");
        body.velocity = new Vector2(body.velocity.x, speed);
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
