using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    public float destroyTime;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("SetDestroy", destroyTime);
    }
    private void SetDestroy()
    {
        Destroy(gameObject);
    }
}
