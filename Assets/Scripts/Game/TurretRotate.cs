using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour
{
    public MoveIC tankMovement;      // check of tank beweegt
    public Transform turretPart;     // deel dat draait
    public float rotateSpeed = 120f; // snelheid turret

    private float rotateInput;       // input voor draaien

    // input voor turret draaien
    public void OnTurretRotate(InputAction.CallbackContext ctx)
    {
        rotateInput = ctx.ReadValue<float>(); // Q E of right stick
    }

    private void Update()
    {
        // turret mag niet draaien als tank beweegt
        if (tankMovement != null && tankMovement.IsMoving)
            return;

        float rot = rotateInput * rotateSpeed * Time.deltaTime;
        turretPart.Rotate(0, rot, 0, Space.Self);
    }
}
