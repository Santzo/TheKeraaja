using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KeraajaController : MonoBehaviour
{
    private int walk, idle, walkMultiplier, pickUp;
    private Animator anim;
    private NavMeshAgent agent;
    int layerMask;
    private Transform right, left, baseLocation;
    private Vector3 currentTarget, currentRullakko;
    public float walkSpeed = 10f;
    public float rotationSpeed = 10f;
    // Start is called before the first frame update
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Default");
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        walk = Animator.StringToHash("Walk");
        idle = Animator.StringToHash("Idle");
        walkMultiplier = Animator.StringToHash("WalkMultiplier");
        pickUp = Animator.StringToHash("PickUp");
        var kone = GameObject.Find("Kerayskone");
        left = kone.transform.Find("Left");
        right = kone.transform.Find("Right");
        baseLocation = kone.transform.Find("KeraajaLocation");
        anim.SetFloat(walkMultiplier, 2.5f);
        Events.onStartCollecting += StartCollecting;
    }

    private void StartCollecting()
    {
        anim.SetTrigger(walk);
        currentTarget = KaytavaManager.currentKeraysTarget;
        var curRul = KeraysKoneController.instance.rullakot[0];
        var leftRullakko = curRul.position + -curRul.right * 0.7f;
        var rightRullakko = curRul.position + curRul.right * 0.7f;
        var whichSide = (currentTarget - leftRullakko).magnitude >= (currentTarget - rightRullakko).magnitude ? rightRullakko : leftRullakko;
        
        var a = Physics.OverlapSphere(whichSide, 0.5f, layerMask);
        if (a.Length > 0)
        {
            Debug.Log("Osui");
            currentRullakko = whichSide == leftRullakko ? rightRullakko : leftRullakko;
        }
        else
        {
            currentRullakko = whichSide;
        }
        agent.destination = currentTarget;
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

        if (!Events.isPlayerPickingUp)
        {
            if (agent.isStopped)
            {
                agent.isStopped = false;
                agent.destination = currentTarget;
            }

            var look = currentTarget - transform.position;
            var dir = Quaternion.LookRotation(look, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, dir, rotationSpeed * Time.deltaTime);
        }
        else
        {
            var time = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (time >= 1f)
            {
                anim.SetTrigger(walk);
                Events.isPlayerPickingUp = false;
                return;
            }

        }
  
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !Events.isPlayerPickingUp)
        {
            anim.SetTrigger(pickUp);
            var look = currentTarget - transform.position;
            var dir = Quaternion.LookRotation(look, transform.up);
            transform.rotation = dir;
            agent.isStopped = true;
            Events.isPlayerPickingUp = true;
            currentTarget = currentTarget == KaytavaManager.currentKeraysTarget ? currentRullakko : KaytavaManager.currentKeraysTarget;
        }



    }
}
