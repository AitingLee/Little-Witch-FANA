using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitAttackFX : MonoBehaviour
{
    [Header("FX")]
    public GameObject firePoint;
    public GameObject LeftAttackFX;
    public GameObject RightAttackFX;
    public GameObject skillAFX;
    public GameObject skillBFX;
    public GameObject skillCFX;
    public GameObject skillDFX;

    PlayerCombatMotion playerCombatMotion;

    private void Start()
    {
        playerCombatMotion = GetComponent<PlayerCombatMotion>();
    }


    public void EmitLeftAttackFX()
    {
        GameObject FX = ObjectPoolManager.instance.GetWave();
        FX.transform.position = firePoint.transform.position;
        FX.transform.rotation = firePoint.transform.rotation;
        FX.transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x + 90, transform.localEulerAngles.y, transform.localEulerAngles.z);
        FX.SetActive(true);
    }

    public void EmitRightAttackFX()
    {
        GameObject FX = ObjectPoolManager.instance.GetMagicBall();
        FX.transform.position = firePoint.transform.position;
        MagicBall magicBall = FX.GetComponent<MagicBall>();
        if (magicBall != null)
        {
            magicBall.direction = PlayerManager.instance.playerCombatMotion.targetPoint;
        }
        FX.SetActive(true);
    }

    public void EmitSkillAFX()  //Às±²­·
    {
        GameObject FX = ObjectPoolManager.instance.GetTornado();
        FX.transform.position = firePoint.transform.position + transform.forward * 1.5f;
        FX.transform.rotation = transform.rotation;
        FX.SetActive(true);
    }

    public void EmitSkillBFX()  //¦BÀ@
    {
        GameObject FX = ObjectPoolManager.instance.GetSpike();
        FX.transform.position = firePoint.transform.position + transform.forward;
        FX.transform.rotation = transform.rotation;
        FX.SetActive(true);
        AudioManager.instance.iceSkill.Play();
    }

    public void EmitSkillCFX()  //¨¾Å@¸n
    {
        if (!skillCFX.activeSelf)
        {
            skillCFX.SetActive(true);
        }
        skillCFX.GetComponent<ParticleSystem>().Play();
    }

    public void EmitSkillDFX()  //¤õ«B
    {
        GameObject FX = ObjectPoolManager.instance.GetFireRain();
        FX.transform.position = transform.position;
        FX.transform.rotation = Quaternion.identity;
        FX.SetActive(true);
    }
}
