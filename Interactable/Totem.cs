using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TotemElement { earth, fire, air, water}

public class Totem : MonoBehaviour
{
    public TotemElement totemElement;
    public MobAI triggerMob;
    public GameObject orb;
    public GameObject totemGO;

    public bool appeared, appearing;
    public Material material;
    private float dissolveValue;
    PlayerCombatMotion combat;

    private void Start()
    {
        totemGO = transform.GetChild(0).gameObject;
        orb = transform.GetChild(1).gameObject;
        dissolveValue = 0f;
        combat = PlayerManager.instance.playerCombatMotion;
    }

    private void Update()
    {
        if (!appeared)
        {
            if (triggerMob.m_isDead)
            {
                totemGO.SetActive(true);
                appearing = true;
            }
        }

        if (appearing)
        {
            Appear();
        }
    }

    private void Appear()
    {
        if (dissolveValue < 0.5f)
        {
            dissolveValue += 0.02f;
            material.SetFloat("_Clipping_Level", dissolveValue);
        }
        else
        {
            material.SetFloat("_Clipping_Level", 0.5f);
            appearing = false;
            orb.SetActive(true);
            appeared = true;
        }
    }

    public void TakeOrb()
    {
        orb.gameObject.SetActive(false);
        combat.LearnSpell(totemElement);
    }

}
