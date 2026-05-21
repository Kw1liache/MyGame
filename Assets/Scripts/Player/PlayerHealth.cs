using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable, IHealth
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1f;

    private int currentHealth;
    private bool isInvincible;
    private PlayerMovement movement;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || movement.IsDead)
            return;

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityFrames());
    }

    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
    }

    private void Die()
    {
        OnDeath?.Invoke();

        GameEventBus.OnPlayerDeath?.Invoke();

        movement.Die();
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void InstantKill()
    {
        currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Die();
    }
}