using UnityEngine;

public class TankBullet : MonoBehaviour
{
    public GameObject owner;      // tank die schoot
    public float LifeTime = 3f;   // hoe lang bullet blijft
    public ParticleSystem Explosion; // explosion effect

    private void Start()
    {
        Destroy(gameObject, LifeTime); // auto verwijderen
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check of je jezelf raakt
        if (collision.gameObject == owner || collision.transform.IsChildOf(owner.transform))
        {
            Destroy(gameObject); // bullet weg
            return;
        }

        // health zoeken
        Health hp = collision.gameObject.GetComponentInParent<Health>();

        // damage doen
        if (hp != null)
            hp.TakeDamage(1);

        // explosion effect
        Instantiate(Explosion, transform.position, Quaternion.identity);

        Destroy(gameObject); // bullet weg
    }
}
