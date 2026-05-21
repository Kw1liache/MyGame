using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHeartsUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private List<Image> hearts = new();

    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHearts;
    }

    private void UpdateHearts(int currentHealth, int maxHealth)
    {
        for (var i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite =
                i < currentHealth
                ? fullHeart
                : emptyHeart;
        }
    }
}