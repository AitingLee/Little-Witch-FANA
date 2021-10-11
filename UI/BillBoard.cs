using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
    }
}
