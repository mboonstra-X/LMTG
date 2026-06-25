using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour
{
    public MoveIC tankMovement;      // check of tank rijdt
    public Transform turretPart;     // turret object
    public float rotateSpeed = 120f; // draai snelheid

    private float rotateInput;       // input waarde

    public void OnTurretRotate(InputAction.CallbackContext ctx)
    {
        rotateInput = ctx.ReadValue<float>(); // Q E of stick
    }

    private void Update()
    {
        if (tankMovement != null && tankMovement.IsMoving)
            return; // niet draaien als tank beweegt

        float rot = rotateInput * rotateSpeed * Time.deltaTime;
        turretPart.Rotate(0, rot, 0, Space.Self); // draai turret
    }
}
