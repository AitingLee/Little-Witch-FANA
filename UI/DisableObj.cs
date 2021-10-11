using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObj : MonoBehaviour
{
    public float disableTime;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("SetDisable", disableTime);
    }
    private void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
