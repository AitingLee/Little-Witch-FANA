using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum eState
    {
        NONE = -1,
        OUTSIDE_TEST,
        INSIDE_TEST,
        COL_TEST
    }

    public float m_fRadius;
    [HideInInspector]
    public eState m_eState = eState.NONE;

    private void Start()
    {
        if (this.GetComponent<SphereCollider>() != null)
        {
            Physics.IgnoreCollision(this.GetComponent<SphereCollider>(), PlayerManager.instance.GetComponent<CapsuleCollider>());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (m_eState == eState.INSIDE_TEST)
        {
            Gizmos.color = Color.yellow;
        }
        else if (m_eState == eState.COL_TEST)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(this.transform.position, m_fRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        MobAI mob = collision.gameObject.transform.GetComponent<MobAI>();
        if (mob != null)
        {
            mob.m_vTargetPosition = mob.m_homePosition;
            mob.m_bMove = true;
        }

        Tornado tornado = collision.gameObject.transform.GetComponent<Tornado>();
        if (tornado != null)
        {
            tornado.m_moveable = false;
        }
    }
}
