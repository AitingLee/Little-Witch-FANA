using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMillFanRotating : MonoBehaviour
{
    public Transform fan;
    Vector3 fanCenter;
    Quaternion rotation;

    private void Start()
    {
        fanCenter = fan.position;
    }
    // Update is called once per frame
    void Update()
    {
        fan.RotateAround(fanCenter, transform.forward, 2f);
    }
}
