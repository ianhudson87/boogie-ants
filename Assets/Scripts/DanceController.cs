using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceController : MonoBehaviour
{
    float frameRate = 24f;
    float frameTime;
    [SerializeField] Ant[] ants;
    Vector3[] offsets;

    Vector3 originalPosition;
    Vector3 originalRotation;

    void Awake() {
        frameTime = 1 / frameRate;

        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;

        offsets = new Vector3[ants.Length];

        for(int i = 0; i < ants.Length; i++) {
            offsets[i] = this.transform.position - ants[i].positionObject.transform.position;
        }
    }

    void Start() {
        StartCoroutine(Dance());
    }

    void Update() {
        MakeAntsCopyLeader();
    }

    IEnumerator Dance() {
        StartCoroutine(Bounce(0.1f, 1f));
        yield return new WaitForSeconds(1f);
        // StartCoroutine(Walk(3f, 30f, true));
        StartCoroutine(Spin(3f, 360f, true));
        yield return new WaitForSeconds(1f);
        StartCoroutine(Bounce(0.1f, 1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(Bounce(0.1f, 1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(Bounce(0.1f, 1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(Bounce(0.1f, 1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(Bounce(0.1f, 1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(Bounce(0.1f, 1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(RandomMoves(1f, new Vector3(3,0,3), new Vector3(15,360,15)));
        yield return new WaitForSeconds(1f);

        StartCoroutine(Dance());
    }

    IEnumerator Bounce(float bounceHeight, float bounceTime)
    {
        int numFrames = (int) (bounceTime / frameTime);
        // Debug.Log("start bounce");
        for(int i = 0; i < numFrames; i++) {
            if(i < numFrames / 4)
                transform.position += new Vector3(0, bounceHeight / (float)(numFrames/4), 0);
            else if (i < numFrames * 3 / 4)
                transform.position -= new Vector3(0, bounceHeight / (float)(numFrames/4), 0);
            else
                transform.position += new Vector3(0, bounceHeight / (float)(numFrames/4), 0);
            yield return new WaitForSeconds(frameTime);
        }
        // Debug.Log("end bounce");
    }

    IEnumerator Spin(float spinTime, float degrees, bool clockwise=true)
    {
        int numFrames = (int) (spinTime / frameTime);
        for(int i = 0; i < numFrames; i++) {
            transform.eulerAngles += new Vector3(0, degrees / numFrames, 0) * (clockwise? 1 : -1);
            yield return new WaitForSeconds(frameTime);
        }
    }

    IEnumerator Walk(float walkTime, float walkSpeed, bool forward=true)
    {
        int numFrames = (int) (walkTime / frameTime);
        for(int i = 0; i < numFrames; i++) {
            transform.position += transform.forward * Time.deltaTime * walkSpeed * (forward? 1 : -1);
            yield return new WaitForSeconds(frameTime);
        }
    }

    IEnumerator RandomMoves(float moveTime, Vector3 maxPosMag, Vector3 maxRotMag)
    {
        int numFrames = (int) (moveTime / frameTime);
        Vector3 deltaPosition = randomVec3(maxPosMag);
        Vector3 deltaRotation = randomVec3(maxRotMag);

        Vector3 originalPosition = transform.position;
        Vector3 originalRotation = transform.eulerAngles;
        
        for(int i = 0; i < numFrames; i++) {
            transform.position = originalPosition + deltaPosition/numFrames * (i+1);
            transform.eulerAngles = originalRotation + deltaRotation/numFrames * (i+1);
            yield return new WaitForSeconds(frameTime);
        }
    }

    void MakeAntsCopyLeader() {
        for(int i = 0; i < ants.Length; i++) {
            ants[i].positionObject.transform.position = this.transform.position - offsets[i];
            ants[i].positionObject.transform.eulerAngles = this.transform.eulerAngles;
        }
    }

    Vector3 randomVec3(Vector3 maxMagnitudes) {
        return new Vector3(
            Random.Range(-maxMagnitudes.x, maxMagnitudes.x),
            Random.Range(-maxMagnitudes.y, maxMagnitudes.y),
            Random.Range(-maxMagnitudes.z, maxMagnitudes.z)
        );
    }
}
