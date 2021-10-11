using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public GameObject[] closePoint;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 1f);
        foreach (GameObject cp in closePoint)
        {
            Gizmos.DrawLine(transform.position, cp.transform.position);
        }
    }
}
