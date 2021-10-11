using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikeUnit : MonoBehaviour
{
	public Vector3 Offset = new Vector3(0, 0.001f, 0);
	public LayerMask layerMask;
	public CapsuleCollider frozenTrigger;
	PlayerCombatMotion combat;
	public List<MobAI> hitMobs;

	void OnEnable()
	{
		combat = PlayerManager.instance.playerCombatMotion;
		hitMobs = new List<MobAI>();
		RaycastHit hit;
		if (Physics.Raycast(transform.position + Vector3.up * 20, -Vector3.up, out hit, 100, layerMask))
		{
			this.transform.position = hit.point + Offset;
			this.transform.forward = hit.normal;
		}
		else
		{
			this.transform.position = transform.position + Offset;
		}
	}

    private void OnTriggerEnter(Collider other)
    {
		Debug.Log("Collide");
		MobAI target = other.GetComponent<MobAI>();
		if (target != null && !target.m_isDead)
        {
			foreach (MobAI mob in hitMobs)
            {
				if (mob.Equals(target))
                {
					return;
                }
            }
			hitMobs.Add(target);
			target.BeFrozen();
			combat.CalculateDamage(combat.IceSpikePower, target, false);
		}
    }

}
