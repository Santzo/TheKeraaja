using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class KeraajaController : MonoBehaviour
{
    private int animWalk, animIdle, animWalkMultiplier, animPickUp, animWalkBox;
    private Animator anim;
    private NavMeshAgent agent;
    int layerMask;
    private Transform right, left, baseLocation, currentRullakko, front;
    private Vector3 currentTarget, currentRullakkoSide, currentKoneSide, currentAktiivi;
    public float walkSpeed = 10f;
    public float rotationSpeed = 10f;
    public GameObject box;
    float collectingSpeed = 1f;
    private float rullakkoSideOffset = 0.65f, minDistance = 0.15f;
    private bool startKerays = false;
    // Start is called before the first frame update
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Default");
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        box.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        AnimationsStringToHash();
        var kone = GameObject.Find("Kerayskone");
        left = kone.transform.Find("Left");
        right = kone.transform.Find("Right");
        front = kone.transform.Find("Front");
        baseLocation = kone.transform.Find("KeraajaLocation");
        anim.SetFloat(animWalkMultiplier, 2.5f);
        Events.onStartCollecting += StartCollecting;
        Events.onTapMovementSpeed += val => collectingSpeed = val;
    }

    private void AnimationsStringToHash()
    {
        animWalk = Animator.StringToHash("Walk");
        animIdle = Animator.StringToHash("Idle");
        animWalkMultiplier = Animator.StringToHash("WalkMultiplier");
        animPickUp = Animator.StringToHash("PickUp");
        animWalkBox = Animator.StringToHash("WalkBox");
    }

    private void StartCollecting(bool start)
    {
        if (!start) return;
        Events.onUpdateRemainingAmount(Events.currentRivi.howManyLeft);
        anim.SetTrigger(animWalk);
        currentAktiivi = KaytavaManager.currentKeraysTarget;
        currentRullakko = KeraysKoneController.instance.rullakot[0];
        var leftRullakko = currentRullakko.position + -currentRullakko.right * rullakkoSideOffset;
        var rightRullakko = currentRullakko.position + currentRullakko.right * rullakkoSideOffset;
        var whichSide = (currentAktiivi - leftRullakko).magnitude >= (currentAktiivi - rightRullakko).magnitude ? rightRullakko : leftRullakko;
        var koneMulti = whichSide == leftRullakko ? -1f : 1f;
        var a = Physics.OverlapSphere(whichSide, 0.5f, layerMask);
        var b = Physics.OverlapSphere(front.position + koneMulti * front.right * rullakkoSideOffset, 0.5f, layerMask);
        if (a.Length > 0 || b.Length > 0)
        {
            Debug.Log("Osui");
            currentRullakkoSide = whichSide == leftRullakko ? rightRullakko : leftRullakko;
        }
        else
        {
            currentRullakkoSide = whichSide;
        }
        currentKoneSide = currentRullakkoSide == leftRullakko ? left.position : right.position;
        currentTarget = currentKoneSide;
        startKerays = true;
        agent.enabled = false;
        transform.rotation = Quaternion.LookRotation(currentTarget - transform.position);
    }
    void RotateTowardsTarget(Vector3? target = null, bool snap = false)
    {
        var _target = target ?? agent.steeringTarget;
        var look = _target - transform.position;
        var dir = Quaternion.LookRotation(look, transform.up);
        transform.rotation = !snap ? Quaternion.RotateTowards(transform.rotation, dir, rotationSpeed * Time.deltaTime)
                                    : dir;
    }
    void Update()
    {
        if (!Events.isPlayerCollecting)
        {
            transform.position = baseLocation.position;
            transform.forward = baseLocation.forward;
            return;
        }

        anim.SetFloat(animWalkMultiplier, collectingSpeed * 0.75f);
        if (startKerays)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, collectingSpeed * Time.deltaTime);
            RotateTowardsTarget(currentTarget);
            if (transform.position == currentTarget)
            {
                startKerays = false;
                currentTarget = currentAktiivi;
                agent.enabled = true;
                agent.isStopped = false;
                agent.destination = currentTarget;
            }
            return;
        }

        agent.speed = collectingSpeed;

        if (!Events.isPlayerPickingUp)
        {
            if (agent.isStopped)
            {
                agent.isStopped = false;
                agent.destination = currentTarget;
            }
            RotateTowardsTarget();
        }

        else
        {
            var time = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (time >= 0.5f && currentTarget == currentRullakkoSide && !box.activeSelf)
            {
                box.SetActive(true);
            }
            else if (time >= 0.5f && currentTarget != currentRullakkoSide && box.activeSelf)
            {
                box.SetActive(false);
            }
            if (time >= 1f)
            {
                anim.SetTrigger(currentTarget == currentRullakkoSide ? animWalkBox : animWalk);
                Events.isPlayerPickingUp = false;
                return;
            }

        }

        var dist = (currentTarget - transform.position).sqrMagnitude;

        if (!agent.pathPending && dist < minDistance * minDistance && !Events.isPlayerPickingUp)
        {
            if (currentTarget == currentKoneSide)
            {
                if (Events.currentRivi.howManyLeft > 0)
                {
                    currentTarget = currentAktiivi;
                    agent.destination = currentTarget;
                }
                else
                {
                  
                    Events.isPlayerCollecting = false;
                    Events.onStartCollecting(false);
                    Events.isPlayerPickingUp = false;
                    agent.enabled = false;
                    anim.SetTrigger(animIdle);
                    Events.seuraavaRivi();
                    return;
                }
            }
            else
            {
                if (currentTarget == currentRullakkoSide)
                {
                    RotateTowardsTarget(currentRullakko.position, true);
                    Events.currentRivi.howManyLeft--;
                    Events.onUpdateRemainingAmount(Events.currentRivi.howManyLeft);
                }
                else
                {
                    RotateTowardsTarget(currentTarget, true);
                }
                anim.SetTrigger(animPickUp);
                agent.isStopped = true;
                Events.isPlayerPickingUp = true;
                currentTarget = currentTarget == currentAktiivi ? currentRullakkoSide : currentAktiivi;
                if (Events.currentRivi.howManyLeft == 0)
                {

                    currentTarget = currentKoneSide;
                }
            }
        }



    }
}
