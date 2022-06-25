using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegJoint : MonoBehaviour
{
    [SerializeField] public GameObject pivot;
    [SerializeField] public GameObject floatingDesiredPosition;

    void Start() {
        floatingDesiredPosition.GetComponent<Renderer>().enabled = false;
    }
}
