using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRain : MonoBehaviour
{
    public float damageGapTime;

    float m_fInsideTime;
    float m_fDamageGapTime = 0.25f;

    PlayerCombatMotion combat;

    float m_fLifeTime;


    private void OnEnable()
    {
        combat = PlayerManager.instance.playerCombatMotion;
        m_fLifeTime = 0;
    }

    private void Update()
    {
        m_fLifeTime += Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MobAI>() != null)
        {
            m_fInsideTime = 0;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<MobAI>() != null)
        {
            MobAI target = other.gameObject.GetComponent<MobAI>();
            if (target != null && !target.m_isDead)
            {
                m_fInsideTime += Time.deltaTime;

                if (m_fInsideTime >= m_fDamageGapTime && m_fLifeTime > 0.5f && m_fLifeTime <2.5f)
                {
                    m_fInsideTime = 0;
                    combat.CalculateDamage(combat.FireRainPower, target, false);
                }
            }
        }
    }
}
