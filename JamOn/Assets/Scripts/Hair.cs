using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hair : MonoBehaviour
{
    LineRenderer lineRend;

    Vector3[] segmentV;

    public int length;
    public Vector3[] segmentPoses;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    public float maxSpeed;

    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }

    void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i],
                segmentPoses[i - 1] + targetDir.right * targetDist + new Vector3(0, -0.002f * Mathf.Pow(i, 2.0f), 0),
                ref segmentV[i], smoothSpeed + i / trailSpeed/*, maxSpeed*/);
        }

        lineRend.SetPositions(segmentPoses);
    }

    public void Tp(Vector3 pos)
    {
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] += pos;
        }
        lineRend.SetPositions(segmentPoses);
    }
}
