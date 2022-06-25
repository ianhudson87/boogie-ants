using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAnimator : MonoBehaviour
{
    private const float frameRate = 60;
    private float frameTime;
    [SerializeField] bool useAltSolution;
    [SerializeField] LegJoint legJoint;
    GameObject floatingDesiredPosition;
    GameObject hipPivot;
    [SerializeField] GameObject legSegment1;
    [SerializeField] GameObject legSegment2;
    [SerializeField] float stepDistanceThreshold;
    [SerializeField] GameObject floatingStartPosition;

    Vector3 desiredPosition = new Vector3();
    Vector3 currentPosition = new Vector3();

    void Start() {
        frameTime = 1 / frameRate;

        floatingDesiredPosition = legJoint.floatingDesiredPosition;
        hipPivot = legJoint.pivot;

        RaycastHit hit;
        if(Physics.Raycast(floatingStartPosition.transform.position, Vector3.down, out hit)) {
            currentPosition = hit.point;
        }
        DoInverseKinematics();
        Destroy(floatingStartPosition);
    }

    void Update() {
        // Project ray from the floating desired position down to the ground. Place the desired position at the point of intersection
        RaycastHit hit;
        if(Physics.Raycast(floatingDesiredPosition.transform.position, Vector3.down, out hit)) {
            desiredPosition = hit.point;
        }

        if((currentPosition - desiredPosition).magnitude > stepDistanceThreshold)
            // currentPosition = desiredPosition; // move the foot if the desired position is far to current position, or we have an invalid position
            StartCoroutine(MoveFootToDesiredPosition(0.1f));

        DoInverseKinematics();
    }

    IEnumerator MoveFootToDesiredPosition(float stepTime) {
        int numFrames = (int) (stepTime / frameTime);
        Vector3 deltaPosition = desiredPosition - currentPosition;
        Vector3 originalPosition = currentPosition;
        for(int i = 0; i < numFrames; i++) {
            currentPosition = originalPosition + (i+1) * deltaPosition / numFrames;
            yield return new WaitForSeconds(frameTime);
        }
    }

    void DoInverseKinematics() {
        // Moves foot of leg to currentPosition

        // Rotate the entire leg to point towards the currentPosition we just set.
        Vector3 deltaPositionGlobal = currentPosition - hipPivot.transform.position;
        float legRotation = -Mathf.Atan(deltaPositionGlobal.z / deltaPositionGlobal.x);
        if(deltaPositionGlobal.x < 0)
            legRotation += Mathf.PI;

        transform.localEulerAngles = new Vector3(0, legRotation / Mathf.PI * 180f, 0);

        // Assume the orientation of the leg
        Vector3 deltaPositionLocal = transform.InverseTransformPoint(currentPosition) - transform.InverseTransformPoint(hipPivot.transform.position);

        // Find the angles needed to bend the leg segments so that leg will touch the desired position
        // using equations found here: https://www.youtube.com/watch?v=RH3iAmMsolo&ab_channel=Woolfrey
        float x = deltaPositionLocal.x;
        float y = deltaPositionLocal.y; // TODO: need to make this account for rotation
        // Debug.Log(x);
        // Debug.Log(y);
        
        float l1 = legSegment1.transform.lossyScale.y;
        float l2 = legSegment2.transform.lossyScale.y;

        // Find the angle for the leg segments
        float q2 = Mathf.Acos((Mathf.Pow(x,2) + Mathf.Pow(y,2) - Mathf.Pow(l1,2) - Mathf.Pow(l2,2)) / (2 * l1 * l2));
        if(useAltSolution)
            q2 *= -1;
        float q1 = Mathf.Atan(x/y) - Mathf.Atan((l2 * Mathf.Sin(q2))/(l1 + l2 * Mathf.Cos(q2)));

        // Debug.Log(q1 / Mathf.PI * 180f);
        // Debug.Log((q1+q2) / Mathf.PI * 180f);
        // Debug.Log("");

        // Rotate the leg segments based on the q2 and q1 values calculated
        if(!float.IsNaN(q1) && !float.IsNaN(q2)) {
            legSegment1.transform.localEulerAngles = new Vector3 (0, 0, -(q1 / Mathf.PI * 180f));
            legSegment2.transform.localEulerAngles = new Vector3 (0, 0, -((q1+q2) / Mathf.PI * 180f));
        }

        // move leg segments to correct positions
        legSegment1.transform.position = hipPivot.transform.position - legSegment1.transform.up * legSegment1.transform.lossyScale.y/2;

        Vector3 midJointPosition = hipPivot.transform.position - legSegment1.transform.up * legSegment1.transform.lossyScale.y;
        legSegment2.transform.position = midJointPosition - legSegment2.transform.up * legSegment2.transform.lossyScale.y/2;
    }
}
