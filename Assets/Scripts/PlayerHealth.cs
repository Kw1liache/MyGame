using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Invincibility")]
    [SerializeField] private float invincibleTime = 1f;

    [SerializeField] private float blinkDelay = 0.1f;

    private int currentHealth;

    private bool isInvincible;

    private SpriteRenderer spriteRenderer;

    private PlayerMovement movement;

    public int CurrentHealth => currentHealth;

    public int MaxHealth => maxHealth;

    public static event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        movement = GetComponent<PlayerMovement>();

        currentHealth = maxHealth;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    public void InstantKill()
    {
        currentHealth = 0;
        UpdateUI();
        Die();
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    private void Die()
    {
        movement.Die();
    }

    private void UpdateUI()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        var timer = 0f;

        while (timer < invincibleTime)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkDelay);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkDelay);
            timer += blinkDelay * 2;
        }

        spriteRenderer.enabled = true;

        isInvincible = false;
    }
}