using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoVortex : MonoBehaviour
{
    public float pullSpeed;

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Mob")
        {
            MobAI mob = other.gameObject.GetComponent<MobAI>();
            if (mob != null && !mob.m_isDead)
            {
                mob.transform.position = Vector3.MoveTowards(mob.transform.position, this.transform.position, pullSpeed * Time.deltaTime);
            }
        }
    }

}
