using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 0.4f;
    [SerializeField] private LayerMask enemyLayer;

    private bool canAttack = true;
    private bool isAttacking;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private PlayerMovement movement;

    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var direction = spriteRenderer.flipX ? -0.45f : 0.45f;

        attackPoint.localPosition =
            new Vector3(direction, attackPoint.localPosition.y, 0);
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed)
            return;

        if (movement.IsDead || !canAttack)
            return;

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        canAttack = true;
    }

    public void DealDamage()
    {
        var hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );

        var damageables = hits
            .Select(hit => hit.GetComponent<IDamageable>())
            .Where(d => d != null)
            .ToList();

        foreach (IDamageable damageable in damageables)
        {
            damageable.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}