using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discoball : MonoBehaviour
{
    [SerializeField] float spinSpeed;

    void Update() {
        transform.eulerAngles += new Vector3(0, spinSpeed, 0) * Time.deltaTime;
    }
}
