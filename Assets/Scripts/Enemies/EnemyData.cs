using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int maxHealth = 3;
    public int damage = 1;
    public float moveSpeed = 2f;
}