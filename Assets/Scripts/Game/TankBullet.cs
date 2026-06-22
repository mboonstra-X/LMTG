using UnityEngine;

public class TankBullet : MonoBehaviour
{
    public GameObject owner; // wie heeft de bullet geschoten

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

        Destroy(gameObject); // bullet weg
    }
}
