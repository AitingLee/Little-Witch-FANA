using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobTrigger : MonoBehaviour
{
    public List<MobAI> triggerMobs;

    void Start()
    {
        triggerMobs = new List<MobAI>();
        foreach (Transform child in transform)
        {
            triggerMobs.Add(child.GetComponent<MobAI>());
        }
        foreach (MobAI mob in triggerMobs)
        {
            if (!mob.m_isAmbush)
            {
                mob.m_isAmbush = true;
                mob.m_FSMSystem.PerformTransition(eFSMTransition.GO_Idle);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (MobAI mob in triggerMobs)
            {
                mob.PopUp();
                RaycastHit hit;
                Vector3 cPos = mob.transform.position;
                Ray ray = new Ray(cPos + Vector3.up * 10, Vector3.down);
                if (Physics.Raycast(ray, out hit, 50f, MobManager.instance.m_groundLayer))
                {
                    cPos.y = hit.point.y + mob.m_fGroundOffset;
                }

                mob.transform.position = cPos;
            }
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
