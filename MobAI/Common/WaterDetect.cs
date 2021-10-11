using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetect : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mob") || other.CompareTag("Elite"))
        {
            MobAI mob = other.GetComponent<MobAI>();
            if (mob != null)
            {
                if (!mob.m_isAmbush)
                {
                    mob.TakeDamage(1000);
                    Debug.Log($"Mob {other.name} is in water");
                }
            }
        }
    }
}
