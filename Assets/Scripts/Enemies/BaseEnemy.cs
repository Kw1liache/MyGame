using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemyData data;

    protected int currentHealth;

    protected virtual void Awake()
    {
        currentHealth = data.maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void DealDamage(PlayerHealth player)
    {
        player.TakeDamage(data.damage);
    }

    protected virtual void Die()
    {
        GameEventBus.OnEnemyKilled?.Invoke();

        Destroy(gameObject);
    }
}