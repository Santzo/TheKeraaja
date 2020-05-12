using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeraysKoneController : MonoBehaviour
{
    Rigidbody rb;
    public static KeraysKoneController instance;
    Vector2 movement;
    WheelCollider[] wheels;
    Vector3 velocity = Vector3.zero;
    internal Transform[] rullakot;
    public float testiRot = 0.1f;
    public float accelerationRate, accRotationRate;
    [SerializeField]
    private float currentMoveSpeed, currentRotationSpeed, maxAngularVelocity;
    [SerializeField]
    public float maxRotationSpeed, maxMoveSpeed;
    [SerializeField]
    private float decelerationRate, decRotationRate, minAccelerationSpeed = 90f, minTurnRate = 130f, angularVel;
    float rotationThreshold = 1f;
    public float strength = 10000f;

    private void Awake()
    {
        if (instance == null) instance = this;
        rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelCollider>();
    }
    private void Start()
    {
        Settings.debugText = GameObject.Find("DebugText").GetComponent<TMPro.TextMeshProUGUI>();
        Spawn();
        Events.applyForce += ApplyForwardForce;
        maxMoveSpeed = Settings.kerayskone.nopeus * 1.65f;
        accelerationRate = Settings.kerayskone.kiihtyvyys * 17.5f;
        decRotationRate = accRotationRate = Settings.kerayskone.kaantyvyys * 3.5f;
        decRotationRate = Settings.kerayskone.jarrutus * 10f;
        rullakot = new Transform[3];
        for (int i = 0; i < rullakot.Length; i++)
        {
            rullakot[i] = transform.GetChild(i);
        }
    }
    private void FixedUpdate()
    {
        if (Events.isPlayerCollecting) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        float rpm = wheels[3].rpm;
        bool brake = ApplyBrake(movement.y, rpm, strength);
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = brake ? strength * 2f : 0f;
            if (i < 2)
            {
                wheels[i].motorTorque = movement.y * strength;
                wheels[i].steerAngle = movement.x * accRotationRate;
            }
        }
    }
    private bool ApplyBrake(float movement, float rpm, float strength)
    {
        if (movement > 0f)
        {
            return rpm > movement * strength;
        }
        else if (movement < 0f)
        {
            return rpm < movement * strength;
        }
        else
        {
            return rpm != movement;
        }
    }
    public void ApplyForwardForce(Vector2 force)
    {
        movement = force;
#if UNITY_EDITOR
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#endif

    }

    void Spawn()
    {
        var obj = Instantiate(Settings.kerayskone.keraysKone, transform, true);
        obj.transform.localPosition = new Vector3(0f, 0f, 0.1f);
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = new Vector3(0.88f, 0.88f, 0.88f);
    }
}
