using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : MonoBehaviour
{
    public float maxHealth = 100f;        // Maximum health of the furniture.
    public float damageInterval = 1.0f;  // Time interval between damage ticks.
    public float damageAmount = 10f;     // Amount of damage to apply per tick.
    public float healAmount = 10f;

    private float currentHealth;         // Current health of the furniture.

    [SerializeField] private Slider slider;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Start the repeating damage function.
        InvokeRepeating("DamageFurniture", 0f, damageInterval);
    }

    private void UpdateHealthBar()
    {
        slider.value = currentHealth / maxHealth;
    }

    private void DamageFurniture()
    {
        // Check if any enemies with the "Enemy" tag are within the collider.
        bool anyEnemies = false;
        bool anyPlayer = false;
        Collider[] colliders = Physics.OverlapBox(transform.position, new(4f, 4f, 4f));
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                anyEnemies = true;
                // Apply damage to the furniture.
                currentHealth -= damageAmount;

                // Check if the furniture is destroyed.
                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    // Perform any actions you want when the furniture is destroyed.
                    Debug.Log("Furniture destroyed!");
                    GameLogic gameLogic = FindObjectOfType<GameLogic>();
                    gameLogic.GameOver();
}

                // Update the health bar text.
            }
            anyPlayer |= collider.CompareTag("Player");
        }
        if (anyPlayer) Debug.Log("Any player:" + anyPlayer + " Any enemy: " + anyEnemies);
        if (anyPlayer && !anyEnemies)
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }
        UpdateHealthBar();
    }
}
