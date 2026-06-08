using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalk : MonoBehaviour
{
    public InputActionAsset inputActions;
    public string actionMapName = "Controls Player2";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Rigidbody rb;

    public float MoveSpeed = 20f;
    public float RotateSpeed = 5f;
    public float JumpSpeed = 5f;

    private InputActionMap map;

    private void OnEnable()
    {
        map = inputActions.FindActionMap(actionMapName);
        map.Enable();
    }

    private void OnDisable()
    {
        map.Disable();
    }

    private void Awake()
    {
        map = inputActions.FindActionMap(actionMapName);

        moveAction = map.FindAction("Move");
        lookAction = map.FindAction("Look");
        jumpAction = map.FindAction("Attack");

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
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