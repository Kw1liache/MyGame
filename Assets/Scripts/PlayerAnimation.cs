using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    private PlayerMovement movement;
    private PlayerAttack attack;

    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int groundedHash = Animator.StringToHash("IsGrounded");
    private readonly int jumpingHash = Animator.StringToHash("IsJumping");
    private readonly int deadHash = Animator.StringToHash("IsDead");
    private readonly int attackingHash = Animator.StringToHash("IsAttacking");

    private void Awake()
    {
        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();

        movement = GetComponent<PlayerMovement>();

        attack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        anim.SetFloat(speedHash, Mathf.Abs(rb.linearVelocity.x));

        anim.SetBool(groundedHash, movement.IsGrounded);

        anim.SetBool(jumpingHash, movement.IsJumping);

        anim.SetBool(deadHash, movement.IsDead);

        anim.SetBool(attackingHash,attack.IsAttacking);
    }
}