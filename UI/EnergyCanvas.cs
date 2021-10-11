using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCanvas : MonoBehaviour
{
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<PlayerManager>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + transform.up * 0.7f + transform.right * 2;
    }
}
