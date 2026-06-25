using UnityEngine;

public class TankBullet : MonoBehaviour
{
    public GameObject owner; // wie heeft de bullet geschoten
    public float LifeTime = 3f; // hoe lang blijft de bullet bestaan
    public ParticleSystem Explosion;

    private void Start()
    {
        Destroy(gameObject, LifeTime); // bullet vernietigen na LifeTime
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check of je jezelf raakt
        if (collision.gameObject == owner || collision.transform.IsChildOf(owner.transform))
        {
            Destroy(gameObject); // bullet weg
            return;
        }

        // zoek health op object of parent
        Health hp = collision.gameObject.GetComponentInParent<Health>();

        // als health bestaat → damage doen
        if (hp != null)
            hp.TakeDamage(1);
        Instantiate(Explosion, transform.position, Quaternion.identity); // explosion prefab

        Destroy(gameObject); // bullet weg
    }
}
