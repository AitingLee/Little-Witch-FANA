using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    public GameObject hit;
    public float lightningProjectileRate;
    public float lightningAttackPower;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            if (hit != null)
            {
                var hitInstance = Instantiate(hit, collisionEvents[i].intersection + collisionEvents[i].normal, new Quaternion()) as GameObject;
                hitInstance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
                var hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                }
            }
        }
        part.Stop();

        if (other.tag == "Player")
        {
            PlayerManager.instance.TakeDamage(Mathf.RoundToInt(lightningAttackPower * lightningProjectileRate));
        }
    }

}
