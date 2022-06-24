using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceController : MonoBehaviour
{
    float frameRate = 24f;
    float frameTime;
    [SerializeField] Ant[] ants;
    Vector3[] offsets;

    void Awake() {
        frameTime = 1 / frameRate;

        offsets = new Vector3[ants.Length];

        for(int i = 0; i < ants.Length; i++) {
            offsets[i] = this.transform.position - ants[i].positionObject.transform.position;
        }
    }

    void Start() {
        StartCoroutine(Dance());
    }

    // void Update() {
    //     S
    // }

    IEnumerator Dance() {
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
        StartCoroutine(Bounce(0.1f, 1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(Bounce(0.1f, 1f));
        yield return new WaitForSeconds(1f);
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

            MakeAntsCopyLeader();
            yield return new WaitForSeconds(frameTime);
        }
        // Debug.Log("end bounce");
    }

    void MakeAntsCopyLeader() {
        for(int i = 0; i < ants.Length; i++) {
            ants[i].positionObject.transform.position = this.transform.position - offsets[i];
            ants[i].positionObject.transform.eulerAngles = this.transform.eulerAngles;
        }
    }
}
