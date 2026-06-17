using UnityEngine;
using UnityEngine.InputSystem;

public class MoveIC : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody rb;
    private int playerId;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float bulletSpeed = 20f;
    public float shootCooldown = 0.3f;
    public ParticleSystem muzzleFlash;

    private float lastShootTime = -999f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // de trygetcomponent komt van Co-Pilot als suggestie om betere performance te hebben dan GetComponent,
        // omdat het een bool teruggeeft in plaats van een exception te gooien als het component niet gevonden wordt.
        if (TryGetComponent<PlayerInput>(out var pi))
            playerId = pi.playerIndex;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    // UNITY EVENTS VERSION
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            Shoot();
    }

    private void Move()
    {
        Vector3 forward = moveInput.y * moveSpeed * Time.fixedDeltaTime * transform.forward;
        rb.MovePosition(rb.position + forward);
    }

    private void Rotate()
    {
        float rotationAmount = moveInput.x * rotationSpeed * Time.fixedDeltaTime;
        Quaternion delta = Quaternion.Euler(0f, rotationAmount, 0f);
        rb.MoveRotation(rb.rotation * delta);
    }

    private void Shoot()
    {
        if (Time.time < lastShootTime + shootCooldown)
            return;

        lastShootTime = Time.time;

        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);

        if (bullet.TryGetComponent<Rigidbody>(out var bulletRb))
            bulletRb.linearVelocity = muzzlePoint.forward * bulletSpeed;

        if (bullet.TryGetComponent<TankBullet>(out var bulletScript))
            bulletScript.shooterId = playerId;

        if (muzzleFlash != null)
            muzzleFlash.Play();
    }
}
