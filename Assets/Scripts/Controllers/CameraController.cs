using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 playerOffset;
    Vector3 oriCamera;
    Vector3 reference = Vector3.zero;
    Transform player;
    Rigidbody playerBody;
    public float cameraTurnSpeed = 500f;
    float cameraFollowSpeed = 0.05f;
    float maxFollowSpeed, followSpeed, offset = 1f;

    private void Start()
    {
        oriCamera = transform.position;
        player = GameObject.Find("Kerayskone").transform;
        playerBody = player.GetComponent<Rigidbody>();
        playerOffset = oriCamera - player.position;
        maxFollowSpeed = cameraFollowSpeed * player.GetComponent<KeraysKoneController>().maxRotationSpeed * 0.75f;
    }
    private void Update()
    {
        float wantedOffset = !Events.isPlayerCollecting ? 1f : 0.82f;
        offset = Mathf.SmoothStep(offset, wantedOffset, 0.025f);
        Vector3 target = player.TransformPoint(playerOffset * offset);
        float playerTurnSpeed = Mathf.Abs(playerBody.angularVelocity.y)+ 1f;
        cameraFollowSpeed = 0.08f - playerBody.velocity.magnitude * 0.004f;
        followSpeed = !Events.isPlayerCollecting ? Mathf.Clamp(cameraFollowSpeed * playerTurnSpeed * 0.125f, 0.02f, maxFollowSpeed)
                                                        : 0.9f;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref reference, followSpeed);
        Quaternion targetRot = new Quaternion();
        targetRot.eulerAngles = new Vector3(transform.eulerAngles.x, player.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, cameraTurnSpeed * Time.deltaTime);
    }
}
