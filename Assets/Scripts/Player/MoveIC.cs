using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MoveIC : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float bulletSpeed = 20f;

    [Header("References")]
    private Rigidbody rb;
    private Vector2 moveInput;

    private float ShootCooldown = 0.5f;
    private float lastShootTime = 0f;

    // krijg de rigidbody component
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // input van de speler opslaan
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    // check of het uitgevoerd wordt en roep shoot aan
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        // check of actie uitgevoerd wordt
        if (ctx.performed)
            Shoot();
    }

    private void FixedUpdate()
    {
        // beweeg de tank op vertical input
        Vector3 forward = transform.forward * moveInput.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forward);

        // rotate de tank op horizontal input
        float rot = moveInput.x * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rot, 0));
    }

    private void Shoot()
    {
        // check of alles ingesteld is
        if (bulletPrefab == null || muzzlePoint == null)
            return;

        // check of de cooldown voorbij is
        if (Time.time - lastShootTime < ShootCooldown)
            return;

        lastShootTime = Time.time;

        // instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);

        // Zet bullet snelheid
        Rigidbody br = bullet.GetComponent<Rigidbody>();
        // check of rigidbody aanwezig is
        if (br != null)
            br.linearVelocity = muzzlePoint.forward * bulletSpeed;
    }
}
