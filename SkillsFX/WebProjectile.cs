using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    public float speed = 15f;
    public GameObject hit;
    private Rigidbody rb;
    public float webProjectileRate;
    public float attackPower;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rb.constraints = RigidbodyConstraints.None;
        speed = 15f;
    }

    void FixedUpdate()
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
        Vector3 pos = contact.point ;

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
        if (collision.gameObject.tag == "Player")
        {
            PlayerManager.instance.TakeDamage(Mathf.RoundToInt(attackPower * webProjectileRate));
        }
    }

}
