using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeraajaController : MonoBehaviour
{
    private int walk, idle, walkMultiplier;
    private Animator anim;
    private Transform right, left, baseLocation;
    private Vector3 currentTarget;
    public float walkSpeed = 10f;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        walk = Animator.StringToHash("Walk");
        idle = Animator.StringToHash("Idle");
        walkMultiplier = Animator.StringToHash("WalkMultiplier");
        var kone = GameObject.Find("Kerayskone");
        left = kone.transform.Find("Left");
        right = kone.transform.Find("Right");
        baseLocation = kone.transform.Find("KeraajaLocation");
        anim.SetFloat(walkMultiplier, 2f);
        Events.onStartCollecting += StartCollecting;
    }

    private void StartCollecting()
    {
        anim.SetTrigger(walk);
        currentTarget = KaytavaManager.currentKeraysTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Events.isPlayerCollecting)
        {
            transform.position = baseLocation.position;
            transform.forward = baseLocation.forward;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, walkSpeed * Time.deltaTime);
        var look = currentTarget - transform.position;
        var dir = Quaternion.LookRotation(look, transform.up);
        transform.rotation = dir;
        if (transform.position == currentTarget)
        {
            currentTarget = currentTarget == KaytavaManager.currentKeraysTarget ? KeraysKoneController.instance.transform.position : KaytavaManager.currentKeraysTarget;
        }


    }
}
