using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeraysKoneController : MonoBehaviour
{
    Rigidbody rb;
    Vector2 movement;
    Transform[] rullakot;
    public float accelerationRate, accRotationRate;
    [SerializeField]
    private float currentMoveSpeed, currentRotationSpeed;
    [SerializeField]
    public float maxRotationSpeed, maxMoveSpeed;
    [SerializeField]
    private float decelerationRate, decRotationRate;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        var obj = Instantiate(Settings.kerayskone.keraysKone, transform, true);
        obj.transform.localPosition = new Vector3(0f, 0f, -0f);
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = new Vector3(0.88f, 0.88f, 0.88f);
        Events.applyForce += ApplyForwardForce;
        maxMoveSpeed = Settings.kerayskone.nopeus * 1.75f;
        accelerationRate = Settings.kerayskone.kiihtyvyys * 17.5f;
        decRotationRate = accRotationRate = Settings.kerayskone.kaantyvyys * 10f;
        decRotationRate = Settings.kerayskone.jarrutus * 10f;
    }
    private void FixedUpdate()
    {
        float currentSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);
        float magnitude = rb.velocity.magnitude;
        float targetSpeed = accelerationRate * movement.y + magnitude * 5f;

        if (movement.y != 0f)
        {
            if (movement.y > 0f) targetSpeed = Mathf.Max(targetSpeed, 90f + magnitude * 2f);
            else targetSpeed = Mathf.Min(targetSpeed, -90f - magnitude * 2f);
            if (magnitude > maxMoveSpeed)
            {
                float oppForce = magnitude - maxMoveSpeed;
                rb.AddForce(-transform.forward * oppForce, ForceMode.Impulse);
            }
            else rb.AddForce(transform.forward * targetSpeed - rb.velocity, ForceMode.Force);
        }
        if (movement.x != 0f)
        {
            var torque = new Vector3(0f, 132.5f * movement.x, 0f);
            rb.AddTorque(torque - rb.angularVelocity, ForceMode.Force);
        }

    }

    public void ApplyForwardForce(Vector2 force)
    {
        movement = force;
#if UNITY_EDITOR
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#endif

    }
    private void OnCollisionEnter(Collision collision)
    {
        currentMoveSpeed = 0f;
    }
   
}
