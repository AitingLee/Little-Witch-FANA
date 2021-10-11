using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjOnDisable : MonoBehaviour
{
    public enum objType {hitFx, wave, magicBall, tornado, spike, spikeUnit, frozenEffect, fireRain, damageNum, 
        questionMark, exclamationMark, angryMark, webProjectile, toxinPoison, pressE, lightOrb, lightSpray, fireLand, fireBall };
    public objType type;

    private void OnDisable()
    {
        switch (type)
        {
            case objType.hitFx:
                ObjectPoolManager.instance.BackHitFX(this.gameObject);
                break;
            case objType.wave:
                ObjectPoolManager.instance.BackWave(this.gameObject);
                break;
            case objType.magicBall:
                ObjectPoolManager.instance.BackMagicBall(this.gameObject);
                break;
            case objType.tornado:
                ObjectPoolManager.instance.BackTornado(this.gameObject);
                break;
            case objType.spike:
                ObjectPoolManager.instance.BackSpike(this.gameObject);
                break;
            case objType.spikeUnit:
                ObjectPoolManager.instance.BackSpikeUnit(this.gameObject);
                break;
            case objType.frozenEffect:
                ObjectPoolManager.instance.BackFrozenEffect(this.gameObject);
                break;
            case objType.fireRain:
                ObjectPoolManager.instance.BackFireRain(this.gameObject);
                break;
            case objType.damageNum:
                ObjectPoolManager.instance.BackDamageNum(this.gameObject);
                break;
            case objType.questionMark:
                ObjectPoolManager.instance.BackQuestionMark(this.gameObject);
                break;
            case objType.exclamationMark:
                ObjectPoolManager.instance.BackExclamationMark(this.gameObject);
                break;
            case objType.angryMark:
                ObjectPoolManager.instance.BackAngryMark(this.gameObject);
                break;
            case objType.webProjectile:
                ObjectPoolManager.instance.BackWebProjectile(this.gameObject);
                break;
            case objType.toxinPoison:
                ObjectPoolManager.instance.BackToxinPoison(this.gameObject);
                break;
            case objType.pressE:
                ObjectPoolManager.instance.BackPressE(this.gameObject);
                break;
            case objType.lightOrb:
                ObjectPoolManager.instance.BackLightOrb(this.gameObject);
                break;
            case objType.lightSpray:
                ObjectPoolManager.instance.BackLightSpray(this.gameObject);
                break;
            case objType.fireLand:
                ObjectPoolManager.instance.BackFireLand(this.gameObject);
                break;
            case objType.fireBall:
                ObjectPoolManager.instance.BackFireBall(this.gameObject);
                break;
        }
    }
}
