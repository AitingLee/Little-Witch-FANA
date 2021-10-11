using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSphereProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float duration = 3f;
    public GameObject hit;
    public float redProjectileRate;
    public float redAttackPower;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private float currentTimer;

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
        //SetDisable();
        part.Stop();

        if (other.tag == "Player")
        {
            PlayerManager.instance.TakeDamage(Mathf.RoundToInt(redAttackPower * redProjectileRate));
        }
    }

    void SetDisable()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
