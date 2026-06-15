using UnityEngine;

public class TankBullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 1f;

    [HideInInspector] public string shooterTag;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Als het dezelfde speler is negeer het
        if (collision.gameObject.CompareTag(shooterTag))
        {
            return;
        }

        // Damage
        Health hp = collision.gameObject.GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
