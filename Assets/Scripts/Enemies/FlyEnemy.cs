using UnityEngine;

public class FlyEnemy : BaseEnemy
{
    [Header("Patrol")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;

    private Transform targetPoint;

    private SpriteRenderer sprite;

    protected override void Awake()
    {
        base.Awake();

        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        targetPoint = rightPoint;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            data.moveSpeed * Time.deltaTime
        );

        sprite.flipX = targetPoint == rightPoint;

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == rightPoint
                ? leftPoint
                : rightPoint;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            DealDamage(player);
        }
    }

    private void OnDrawGizmos()
    {
        if (leftPoint == null || rightPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(leftPoint.position, rightPoint.position);
    }
}