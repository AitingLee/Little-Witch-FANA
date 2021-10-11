using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinLightning : MonoBehaviour
{
    float stayTime;
    public float damageTime = 0.5f;
    public float spinRate;
    public float attackPower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stayTime = 0.5f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stayTime += Time.deltaTime;
            Debug.Log($"stayTime = {stayTime}");
            if (stayTime >= damageTime)
            {
                PlayerManager player = other.GetComponent<PlayerManager>();
                if (player != null && player.playerData.HP >= 0)
                {
                    player.TakeDamage(Mathf.RoundToInt(attackPower * spinRate));
                    stayTime = 0;
                }
            }
        }
    }

}
