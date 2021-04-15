using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingRing : MonoBehaviour
{

    Vector3[] segmentV;

    public int length;
    public Vector3[] segmentPoses;

    public Transform target;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;
    [SerializeField] float chargeTime = 0.5f;

    private int nSegment;
    private bool chargingUp = false;
    private float chargeTimer = 0.0f;
    private Transform chargeTransform;

    void Start()
    {
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
        nSegment = length - 1;
    }

    void Update()
    {

        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = target.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i],
                segmentPoses[i - 1] + target.right * targetDist + new Vector3(0, -0.0002f * Mathf.Pow(i, 2.0f), 0),
                ref segmentV[i], smoothSpeed + i / trailSpeed);
        }

        if (chargingUp)
        {
            transform.position = Vector3.Lerp(segmentPoses[nSegment], chargeTransform.position, chargeTimer / chargeTime);
            chargeTimer += Time.deltaTime;
        }
        else if(!chargingUp)
            transform.position = segmentPoses[nSegment];
    }

    public void Teleport()
    {
        segmentPoses[0] = target.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = segmentPoses[i - 1] + target.right * targetDist + new Vector3(0, -0.0002f * Mathf.Pow(i, 2.0f));
        }
        transform.position = segmentPoses[nSegment];
    }

    public void ResetRing()
    {
        gameObject.SetActive(true);
        nSegment = length - 1;
        chargingUp = false;
    }

    public void GetCloser(Transform target)
    {
        chargeTransform = target;
        if (!chargingUp)
        {
            chargingUp = true;
            chargeTimer = 0.0f;
        }
    }
}
