using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MoveIC : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;       // snelheid vooruit/achteruit
    public float rotationSpeed = 120f; // draai snelheid

    [Header("Shooting")]
    public GameObject bulletPrefab;    // bullet prefab
    public Transform muzzlePoint;      // waar bullet uit komt
    public float bulletSpeed = 20f;    // snelheid bullet

    public AudioClip shootsound;     // schiet geluid
    public AudioSource MoveSound;     // beweeg geluid

    private Rigidbody rb;              // tank rigidbody
    private Vector2 moveInput;         // input van speler

    private float ShootCooldown = 0.5f; // tijd tussen schoten
    private float lastShootTime = 0f;   // laatste schot tijd

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // pak rigidbody
    }

    // movement input opslaan
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    // schieten als knop wordt ingedrukt
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            Shoot();
    }

    private void FixedUpdate()
    {
        // vooruit/achteruit bewegen
        Vector3 forward = transform.forward * moveInput.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forward);

        // Driving Sound
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

        // bullet maken
        AudioSource.PlayClipAtPoint(shootsound, muzzlePoint.position, 1f); // speel schiet geluid
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);

        // ⭐ bullet weet wie hem heeft geschoten
        bullet.GetComponent<TankBullet>().owner = gameObject;

        // bullet snelheid
        Rigidbody br = bullet.GetComponent<Rigidbody>();
        if (br != null)
            br.linearVelocity = muzzlePoint.forward * bulletSpeed;
    }
}
