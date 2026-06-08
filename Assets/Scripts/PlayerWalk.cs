using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalk : MonoBehaviour
{
    public InputActionAsset inputActions;
    public string actionMapName = "Player1"; // Set per player in Inspector

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Rigidbody rb;
    private InputActionMap map;

    public float MoveSpeed = 20f;
    public float RotateSpeed = 5f;
    public float JumpSpeed = 5f;

    private void OnEnable()
    {
        map = inputActions.FindActionMap(actionMapName);

        if (map == null)
        {
            Debug.LogError("Action map NOT FOUND: " + actionMapName);
            return;
        }

        map.Enable();
    }

    private void OnDisable()
    {
        if (map != null)
            map.Disable();
    }

    private void Awake()
    {
        map = inputActions.FindActionMap(actionMapName);

        if (map == null)
        {
            Debug.LogError("Action map NOT FOUND in Awake: " + actionMapName);
            return;
        }

        moveAction = map.FindAction("Move");
        lookAction = map.FindAction("Look");
        jumpAction = map.FindAction("Jump");

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (moveAction == null) return;

        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame())
            Jump();
    }

    private void FixedUpdate()
    {
        Walking();
        Rotating();
    }

    private void Walking()
    {
        Vector3 forwardMove = transform.forward * moveInput.y * MoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);
    }

    private void Rotating()
    {
        float rotationAmount = lookInput.x * RotateSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
    }
}
