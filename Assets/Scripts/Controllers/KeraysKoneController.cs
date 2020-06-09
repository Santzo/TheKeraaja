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
    internal RullakkoController[] rullakkoControllers;

    private int howManyBoxesCollected = 0, maxBoxesPerRullakko = 18, currentRullakkoIndex = 0;
    private float _jarrutus, _kiihtyvyys, _maksimiNopeus, _kaantyvyys;

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
        Events.boxCollected += BoxCollected;
        _jarrutus = Settings.kerayskone.jarrutus * 75f;
        _kiihtyvyys = Settings.kerayskone.kiihtyvyys * 47.5f;
        _maksimiNopeus = Settings.kerayskone.nopeus * 125f;
        _kaantyvyys = Settings.kerayskone.kaantyvyys * 7f;
        rullakot = new Transform[3];
        rullakkoControllers = new RullakkoController[3];
        for (int i = 0; i < rullakot.Length; i++)
        {
            rullakot[i] = transform.GetChild(i);
            rullakkoControllers[i] = rullakot[i].GetComponent<RullakkoController>();
        }
        Events.currentRullakko = rullakot[0];
    }

    private void BoxCollected()
    {
        howManyBoxesCollected++;
        rullakkoControllers[currentRullakkoIndex].ActivateBox();
        if (howManyBoxesCollected >= maxBoxesPerRullakko)
        {
            currentRullakkoIndex++;
            howManyBoxesCollected = 0;
        }
        Events.currentRullakko = rullakot[currentRullakkoIndex];

    }

    private void FixedUpdate()
    {
        if (Events.isPlayerCollecting)
        {
            if (rb.velocity.sqrMagnitude > 0.001f)
            {
                ForceBreak();
            }
            return;
        }
        HandleMovement();
    }

    private void HandleMovement()
    {
        float rpm = wheels[3].rpm;
        bool brake = ApplyBrake(movement.y, rpm);
        float kaantyvyys = _kaantyvyys * (1.25f - Mathf.Log(Math.Abs(rpm), _maksimiNopeus));
        float finalKaantyvyys = Mathf.Min(kaantyvyys, 25f);
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = brake ? _jarrutus : 0f;
            if (i < 2)
            {
                wheels[i].motorTorque = movement.y * _kiihtyvyys;
                wheels[i].steerAngle = movement.x * finalKaantyvyys;
            }
        }
    }
   
    private void ForceBreak()
    {
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = 10000f;
        }
        rb.velocity = new Vector3(0f, 0f, 0f);
    }
    private bool ApplyBrake(float movement, float rpm)
    {
        float absRpm = Math.Abs(rpm);
        if (absRpm > _maksimiNopeus) return true;
      
        switch (movement)
        {
            case var _ when movement < 0f:
                if (rpm > 0f)
                    return true;
                else
                    return false;
            case var _ when movement > 0f:
                if (rpm < 0f)
                    return true;
                else return false;
            default:
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
