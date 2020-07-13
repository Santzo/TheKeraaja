using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrukkiManager : MonoBehaviour
{
    public Transform trukki;
    private Path[] paths;
    private Trukki[] trukit;
    private int trukitLength;
    private float trukkiforce = 210000f;
    // Start is called before the first frame update
    void Start()
    {
        paths = new Path[transform.childCount];
        trukit = new Trukki[transform.childCount];
        trukitLength = transform.childCount;
        for (int i = 0; i < paths.Length; i++)
        {
            var child = transform.GetChild(i);
            var points = child.childCount;
            paths[i].points = new Vector3[points];
            for (int j = 0; j < points; j++)
            {
                paths[i].points[j] = child.GetChild(j).transform.position;
            }
            float xPos = Random.Range(paths[i].points[0].x, paths[i].points[1].x);
            float zPos = paths[i].points[0].z;
            trukit[i] = new Trukki();
            trukit[i].trukki = Instantiate(trukki, new Vector3(xPos, 0f, zPos), Quaternion.identity);
            trukit[i].trukkiRB = trukit[i].trukki.GetComponent<Rigidbody>();
            trukit[i].target = Random.Range(0, 2);
            var rotation = Quaternion.LookRotation(paths[i].points[trukit[i].target] - trukit[i].trukki.position, Vector3.up);
            trukit[i].trukki.rotation = rotation;
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < trukitLength; i++)
        {
            var diff = paths[i].points[trukit[i].target] - trukit[i].trukki.position;

            var lookRotation = Quaternion.LookRotation(diff, Vector3.up);
            var moveRotation = Quaternion.Lerp(trukit[i].trukkiRB.rotation, lookRotation, 1.5f * Time.fixedDeltaTime);
            var angle = Quaternion.Angle(lookRotation, trukit[i].trukkiRB.rotation);
            float dist = diff.sqrMagnitude;
            if (angle > 5f)
                trukit[i].trukkiRB.drag = 5f;
            else
                trukit[i].trukkiRB.drag = 0f;
            if (dist < 250f && angle <= 5f)
            {
                var drag = (250f - dist) * 0.006f;
                trukit[i].trukkiRB.drag = drag;
            }
            trukit[i].trukkiRB.MoveRotation(moveRotation);
            var direction = diff.normalized;
            trukit[i].trukkiRB.AddForce(trukit[i].trukki.forward * trukkiforce - trukit[i].trukkiRB.velocity);
            if (dist <= 25f)
            {
                trukit[i].target = trukit[i].target == 0 ? 1 : 0;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
public struct Path
{
    public Vector3[] points;
}
public class Trukki
{
    public int target;
    public Transform trukki;
    public Rigidbody trukkiRB;
}