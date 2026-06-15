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

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} is destroyed!");
        Destroy(gameObject);
    }
}
