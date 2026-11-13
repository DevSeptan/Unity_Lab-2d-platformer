using UnityEngine;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public enum MovementAxis
    {
        Horizontal,
        Vertical
    }

    [Header("Движение")]
    [SerializeField] private MovementAxis axis;
    [SerializeField, Min(0f)] private float distance = 3f;
    [SerializeField, Min(0f)] private float speed = 2f;

    [Header("Остановка в конечных точках")]
    [SerializeField, Min(0f)] private float pauseTime = 1f;

    private Rigidbody2D rb;
    private Vector2 startPosition;

    private bool movingToEnd = true;
    private float pauseRemaining;

    public Vector2 Velocity { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        startPosition = rb.position;
    }

    private void FixedUpdate()
    {
        Velocity = Vector2.zero;

        if (pauseRemaining > 0f)
        {
            pauseRemaining -= Time.fixedDeltaTime;
            return;
        }

        if (distance <= 0f || speed <= 0f)
            return;

        Vector2 direction = axis == MovementAxis.Horizontal
            ? Vector2.right
            : Vector2.up;

        Vector2 target = movingToEnd
            ? startPosition + direction * distance
            : startPosition;

        Vector2 nextPosition = Vector2.MoveTowards(
            rb.position,
            target,
            speed * Time.fixedDeltaTime
        );

        Velocity = (nextPosition - rb.position) / Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);

        if ((nextPosition - target).sqrMagnitude < 0.000001f)
        {
            movingToEnd = !movingToEnd;
            pauseRemaining = pauseTime;
        }
    }
}