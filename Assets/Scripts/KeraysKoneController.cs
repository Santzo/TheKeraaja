﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeraysKoneController : MonoBehaviour
{
    Rigidbody rb;
    Vector2 movement;

    public float accelerationRate, accRotationRate;
    [SerializeField]
    private float currentMoveSpeed, currentRotationSpeed;
    [SerializeField]
    public float maxRotationSpeed, maxMoveSpeed;
    [SerializeField]
    private float decelerationRate, decRotationRate;
    Transform steeringWheel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        steeringWheel = transform.Find("Torus");
    }
    private void Start()
    {
        Events.applyForce += ApplyForwardForce;
    }
    private void FixedUpdate()
    {
        float targetSpeed = maxMoveSpeed * movement.y;
        float accelerate = 0f;
        if (currentMoveSpeed < targetSpeed) accelerate = accelerationRate;
        else accelerate = -accelerationRate;
        currentMoveSpeed += accelerate;

        float targetRot = maxRotationSpeed * movement.x;
        float rotAccelerate = 0f;
        if (currentRotationSpeed < targetRot) rotAccelerate = accRotationRate;
        else rotAccelerate = -accRotationRate;

        float speedEffect = 1.35f - (currentMoveSpeed / maxMoveSpeed);
        rotAccelerate *= speedEffect;

        if (movement.x > 0 && currentRotationSpeed < 20) rotAccelerate *= 2.5f;
        else if (movement.x < 0 && currentRotationSpeed > -20) rotAccelerate *= 2.5f;
        currentRotationSpeed += rotAccelerate;

        if (currentMoveSpeed != 0)
        {
            rb.velocity = transform.forward * currentMoveSpeed * Time.fixedDeltaTime;
        }
        rb.angularVelocity = new Vector3(0f, currentRotationSpeed * Time.fixedDeltaTime, 0f);
    }

    private void LateUpdate()
    {
        if (rb.angularVelocity.y != 0f)
        {
            steeringWheel.Rotate(Vector3.forward * -movement.x * Mathf.Abs(rb.angularVelocity.y) * 30f * Time.deltaTime);
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
