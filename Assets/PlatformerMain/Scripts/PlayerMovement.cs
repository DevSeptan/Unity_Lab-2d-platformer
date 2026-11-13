using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 8f;
    [SerializeField] private float jumpSpeed = 12f;

    [Header("Проверка земли")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Анимация")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private MovingPlatform currentPlatform;

    private float moveInput;
    private bool jumpRequested;
    private float previousPlatformSpeed;
    private float groundCheckBlockedUntil;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        ReadInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
        HandleJump();
    }

    private void ReadInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        jumpRequested |= Input.GetKeyDown(KeyCode.Space);
    }

    private void CheckGround()
    {
        IsGrounded = false;
        currentPlatform = null;

        if (groundCheck == null || Time.time < groundCheckBlockedUntil)
            return;

        Collider2D ground = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        if (ground == null)
            return;

        IsGrounded = true;

        if (ground.attachedRigidbody != null)
        {
            currentPlatform =
                ground.attachedRigidbody.GetComponent<MovingPlatform>();
        }
    }

    private void Move()
    {
        Vector2 platformVelocity = currentPlatform != null
            ? currentPlatform.Velocity
            : Vector2.zero;

        float currentSpeed = rb.linearVelocity.x;

        if (IsGrounded)
            currentSpeed -= previousPlatformSpeed;

        float changeRate = Mathf.Abs(moveInput) > 0.01f
            ? acceleration
            : deceleration;

        float horizontalSpeed = Mathf.MoveTowards(
            currentSpeed,
            moveInput * moveSpeed,
            changeRate * Time.fixedDeltaTime
        );

        float verticalSpeed = rb.linearVelocity.y;

        if (IsGrounded && verticalSpeed <= platformVelocity.y + 0.1f)
            verticalSpeed = platformVelocity.y;

        rb.linearVelocity = new Vector2(
            horizontalSpeed + platformVelocity.x,
            verticalSpeed
        );

        previousPlatformSpeed = platformVelocity.x;
    }

    private void HandleJump()
    {
        if (jumpRequested && IsGrounded)
        {
            float platformVerticalSpeed = currentPlatform != null
                ? currentPlatform.Velocity.y
                : 0f;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpSpeed + Mathf.Max(0f, platformVerticalSpeed)
            );

            IsGrounded = false;
            currentPlatform = null;

            // Не считаем персонажа стоящим на земле сразу после прыжка.
            groundCheckBlockedUntil = Time.time + 0.1f;
        }

        jumpRequested = false;
    }

    private void UpdateAnimation()
    {
        // Учитываем движение относительно платформы.
        float relativeSpeed = rb.linearVelocity.x - previousPlatformSpeed;

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(relativeSpeed));
            animator.SetBool("IsGrounded", IsGrounded);
        }

        if (spriteRenderer != null && Mathf.Abs(relativeSpeed) > 0.1f)
        {
            // Исходный спрайт смотрит вправо.
            spriteRenderer.flipX = relativeSpeed < 0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}