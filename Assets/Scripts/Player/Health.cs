using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 3f;
    private float current;

    private void Awake()
    {
        current = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        current -= amount;
        if (current <= 0)
            Destroy(gameObject);
    }
}
