using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;

    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;

    private Transform targetPoint;

    private SpriteRenderer sprite;

    private void Awake()
    {
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
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (targetPoint == rightPoint)
            sprite.flipX = true;

        else
            sprite.flipX = false;

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == rightPoint)
                targetPoint = leftPoint;
                
            else
                targetPoint = rightPoint;
        }
    }

    private void OnDrawGizmos()
    {
        if (leftPoint == null || rightPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(
            leftPoint.position,
            rightPoint.position
        );
    }
}