using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MoveIC : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        walk();
        Rotate();
    }
    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        Debug.Log(lookInput);
    }
    private void walk()
    {
        rb.MovePosition(rb.position + transform.forward * moveInput.y * moveSpeed * Time.fixedDeltaTime * 5f);
    }

    private void Rotate()
    {
        //if (moveInput.y != 0)
        //{
            Debug.Log(lookInput);
        float rotationAmount = lookInput.x * rotationSpeed * Time.fixedDeltaTime;
            Quaternion rot = Quaternion.Euler(0, rotationAmount, 0);
            rb.MoveRotation(rb.rotation * rot);
        //}
    }
}
