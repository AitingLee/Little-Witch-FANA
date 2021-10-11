using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : MonoBehaviour
{
	public int Number = 10;
	public float Frequency = 1;
	public float TimeSpawn = 0.02f;
	private float timeTemp;
	public Vector3 Direction = Vector3.forward;
	public Vector3 Noise = Vector3.zero;
	public Vector3 Offset = new Vector3(0, 0.001f, 0);
	public GameObject[] spikes = new GameObject[10];

	void OnEnable()
	{
		counter = 0;
		timeTemp = Time.time;
		spikes = new GameObject[10];
		spikes = ObjectPoolManager.instance.Get10SpikeUnit();
	}

	private int counter = 0;

	void Update()
	{
		if (counter >= Number - 1)
		{
			this.gameObject.SetActive(false);
		}

		if (TimeSpawn > 0.0f)
		{
			if (Time.time > timeTemp + TimeSpawn)
			{
				Direction = this.transform.forward + (new Vector3(this.transform.right.x * Random.Range(-Noise.x, Noise.x), this.transform.right.y * Random.Range(-Noise.y, Noise.y), this.transform.right.z * Random.Range(-Noise.z, Noise.z)) * 0.01f);
				spikes[counter].transform.position = transform.position + (Direction * Frequency * counter);
				spikes[counter].SetActive(true);
				counter += 1;
				timeTemp = Time.time;
			}
		}
	}
}
