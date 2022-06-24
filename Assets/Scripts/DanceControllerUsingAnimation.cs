using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceControllerUsingAnimation : MonoBehaviour
{
    [SerializeField] Ant[] ants;
    Vector3[] offsets;

    [SerializeField] GameObject danceLeader;

    void Awake() {
        offsets = new Vector3[ants.Length];

        for(int i = 0; i < ants.Length; i++) {
            offsets[i] = danceLeader.transform.position - ants[i].positionObject.transform.position;
        }
    }

    void Update() {
        MakeAntsCopyLeader();
    }

    void MakeAntsCopyLeader() {
        for(int i = 0; i < ants.Length; i++) {
            ants[i].positionObject.transform.position = danceLeader.transform.position - offsets[i];
            ants[i].positionObject.transform.eulerAngles = danceLeader.transform.eulerAngles + new Vector3(90, 0, 0);
        }
    }
}
