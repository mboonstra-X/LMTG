using UnityEngine;
using UnityEngine.InputSystem;

public class TankBullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 1f;

    [HideInInspector] public int shooterId;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerInput pi = collision.gameObject.GetComponent<PlayerInput>();
        if (pi != null && pi.playerIndex == shooterId)
            return;

        Health hp = collision.gameObject.GetComponent<Health>();
        if (hp != null)
            hp.TakeDamage(damage);

        Destroy(gameObject);
    }
}
