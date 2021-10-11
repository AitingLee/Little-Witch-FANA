using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public GameObject hit;
    private Rigidbody rb;
    public Vector3 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {   
        rb.constraints = RigidbodyConstraints.None;
        speed = 15f;
        transform.LookAt(direction);
    }

    void FixedUpdate ()
    {
		if (speed != 0)
        {
            rb.velocity = transform.forward * speed; 
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            hitInstance.transform.LookAt(contact.point + contact.normal);
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        gameObject.SetActive(false);
        MobAI target = collision.gameObject.GetComponent<MobAI>();
        if (target != null && !target.m_isDead)
        {
            PlayerCombatMotion combat = PlayerManager.instance.playerCombatMotion;
            combat.CalculateDamage(combat.RAttackPower, target, false);
            if (target.mobtype != MobType.dragon)
            {
                target.HandleHitBack();
            }
        }
    }
}
