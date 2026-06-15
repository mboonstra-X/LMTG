using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalk : MonoBehaviour
{
    // input action asset en action map naam
    public InputActionAsset inputActions;
    public string actionMapName = "Player1";
    // input actions
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction shootAction;
    // inputs
    private Vector2 moveInput;
    private Vector2 lookInput;
    // rigidbody van speler
    private Rigidbody rb;
    // parameters voor beweging
    public float MoveSpeed = 7f;
    public float RotateSpeed = 120f;

    // parameters voor schieten
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float bulletSpeed = 20f;

    public float shootCooldown = 0.3f;
    private float lastShootTime = 0f;

    public ParticleSystem muzzleFlash; // voor straks



    // de action map
    private InputActionMap map;

    // zorg dat de input actions weer gestart worden als player levend is
    private void OnEnable()
    {
        map = inputActions.FindActionMap(actionMapName);
        if (map != null)
            map.Enable();
    }

    // zorg dat de input actions worden gestopt als player dood is
    private void OnDisable()
    {
        if (map != null)
            map.Disable();
    }

    // maak de input actions en haal de rigidbody op
    private void Awake()
    {
        map = inputActions.FindActionMap(actionMapName);

        moveAction = map.FindAction("Move");
        lookAction = map.FindAction("Look");
        shootAction = map.FindAction("Attack");

        rb = GetComponent<Rigidbody>();
    }

    // lees input bij elke frame en check of er geschoten wordt
    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        if (shootAction.WasPressedThisFrame())
            Shoot();
    }

    // roep de move en rotate funcite aan
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    // beweging gebasseerd op de y waarde van move input en forward direction van player
    private void Move()
    {
        Vector3 forwardMove = transform.forward * moveInput.y * MoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);
    }

    // De rotatie van x waarde gebasseerd op de look input en rotate rond de y as
    private void Rotate()
    {
        float rotationAmount = lookInput.x * RotateSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void Shoot()
    {
        if (Time.time < lastShootTime + shootCooldown)
            return; // cooldown

        lastShootTime = Time.time;

        // bullet spawn
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
            bulletRb.linearVelocity = muzzlePoint.forward * bulletSpeed;

        // Muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Geef de bullet mee wie hem heeft geschoten
        TankBullet bulletScript = bullet.GetComponent<TankBullet>();
        if (bulletScript != null)
            bulletScript.shooterTag = gameObject.tag;
    }
}
