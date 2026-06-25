using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MoveIC : MonoBehaviour
{
    public bool IsMoving { get; private set; } // check of tank beweegt

    [Header("Movement")]
    public float moveSpeed = 5f;       // vooruit snelheid
    public float rotationSpeed = 120f; // draai snelheid

    [Header("Shooting")]
    public GameObject bulletPrefab;    // bullet prefab
    public Transform muzzlePoint;      // plek waar bullet uit komt
    public float bulletSpeed = 20f;    // snelheid bullet

    public AudioClip shootsound;       // schiet geluid
    public AudioSource MoveSound;      // rij geluid

    public ParticleSystem muzzleFlash; // muzzle flash effect

    private Rigidbody rb;              // rigidbody tank
    private Vector2 moveInput;         // input speler

    private float ShootCooldown = 0.5f; // tijd tussen schoten
    private float lastShootTime = 0f;   // laatste schot tijd

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // pak rigidbody
    }

    // input voor bewegen
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    // input voor schieten
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            Shoot();
    }

    private void FixedUpdate()
    {
        IsMoving = moveInput.x != 0 || moveInput.y != 0;

        // vooruit en achteruit
        Vector3 forward = transform.forward * moveInput.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forward);

        // rij geluid
        if (moveInput.y != 0 && !MoveSound.isPlaying)
        {
            MoveSound.loop = true;
            MoveSound.Play();
        }
        else if (moveInput.y == 0 && MoveSound.isPlaying)
        {
            MoveSound.Stop();
        }

        // draaien
        float rot = moveInput.x * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rot, 0));
    }

    private void Shoot()
    {
        // check of alles bestaat
        if (bulletPrefab == null || muzzlePoint == null)
            return;

        // cooldown check
        if (Time.time - lastShootTime < ShootCooldown)
            return;

        lastShootTime = Time.time;

        // schiet geluid
        AudioSource.PlayClipAtPoint(shootsound, muzzlePoint.position, 1f);

        // bullet maken
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);

        // geef bullet de eigenaar
        bullet.GetComponent<TankBullet>().owner = gameObject;

        // bullet snelheid
        Rigidbody br = bullet.GetComponent<Rigidbody>();
        if (br != null)
            br.linearVelocity = muzzlePoint.forward * bulletSpeed;

        // muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();
    }
}
