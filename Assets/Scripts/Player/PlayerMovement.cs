using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 4.5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Respawn")]
    [SerializeField] private RespawnManager respawnManager;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerHealth health;

    private float moveInput;

    private bool isGrounded;
    private bool jumpPressed;
    private bool isJumping;
    private bool isDead;

    public bool IsGrounded => isGrounded;
    public bool IsJumping => isJumping;
    public bool IsDead => isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        CheckGround();
        Move();
        Jump();
    }

    private void Update()
    {
        Flip();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (!jumpPressed || !isGrounded)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        jumpPressed = false;
        isGrounded = false;
        isJumping = true;
    }

    private void Flip()
    {
        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer
        );

        if (isGrounded)
            isJumping = false;
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        moveInput = 0;

        rb.linearVelocity = Vector2.zero;

        rb.constraints = RigidbodyConstraints2D.FreezePosition;

        StartCoroutine(respawnManager.RespawnPlayer(this));
    }

    public void Respawn(Vector3 position)
    {
        transform.position = position;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        rb.linearVelocity = Vector2.zero;

        health.RestoreHealth();

        isDead = false;
        isJumping = false;
        moveInput = 0;
    }

    public void OnMove(InputValue value)
    {
        if (isDead)
            return;

        moveInput = value.Get<Vector2>().x;
    }

    public void OnJump(InputValue value)
    {
        if (isDead)
            return;

        if (value.isPressed && isGrounded)
            jumpPressed = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            health.InstantKill();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}