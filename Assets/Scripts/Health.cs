using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
