using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField, Min(0f)] private float speed = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool spriteFacesRight = true;

    private Rigidbody2D rb;
    private bool movingToB = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (pointA == null || pointB == null || speed <= 0f)
            return;

        float targetX = movingToB ? pointB.position.x : pointA.position.x;
        Vector2 position = rb.position;
        float nextX = Mathf.MoveTowards(
            position.x, targetX, speed * Time.fixedDeltaTime
        );

        rb.MovePosition(new Vector2(nextX, position.y));

        if (spriteRenderer != null && nextX != position.x)
        {
            bool movingRight = nextX > position.x;
            spriteRenderer.flipX = movingRight != spriteFacesRight;
        }

        if (nextX == targetX)
            movingToB = !movingToB;
    }
}