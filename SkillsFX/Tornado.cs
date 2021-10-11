using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    Vector3 dir;
    float m_fInTornadoTime;
    float m_fDamageGapTime = 0.25f;
    public bool m_moveable;
    List<MobAI> mobsFlyInside;
    PlayerCombatMotion combat;

    void OnEnable()
    {
        if (PlayerManager.instance.playerCombatMotion != null)
        {
            combat = PlayerManager.instance.playerCombatMotion;
            transform.forward = combat.transform.forward;
            dir = transform.forward;
            dir.y = 0;
            dir.Normalize();
            m_moveable = true;
            mobsFlyInside = new List<MobAI>();
        }
    }
    void Update()
    {
        if (m_moveable)
        {
            transform.position += dir * 5 * Time.deltaTime;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mob")
        {
            m_fInTornadoTime = 0;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Mob")
        {
            MobAI target = other.gameObject.GetComponent<MobAI>();
            if (target != null && !target.m_isDead)
            {
                mobsFlyInside.Add(target);
                m_fInTornadoTime += Time.deltaTime;
                if (m_fInTornadoTime >= m_fDamageGapTime)
                {
                    m_fInTornadoTime = 0;
                    combat.CalculateDamage(combat.TornadoPower, target, false);
                }
            }
        }
        if (other.gameObject.tag == "Elite")
        {
            MobAI target = other.gameObject.GetComponent<MobAI>();
            if (target != null && !target.m_isDead)
            {
                m_fInTornadoTime += Time.deltaTime;
                if (m_fInTornadoTime >= m_fDamageGapTime)
                {
                    m_fInTornadoTime = 0;
                    combat.CalculateDamage(combat.TornadoPower, target, false);
                }
            }
        }
    }

    public void OnDisable()
    {
        if (mobsFlyInside != null)
        {
            foreach (MobAI mob in mobsFlyInside)
            {
                mob.ExitTornado();
            }
        }
    }
}
