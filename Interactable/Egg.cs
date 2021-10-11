using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public GameObject egg;
    public GameObject egglet;
    public ParticleSystem breakFX;
    public SphereCollider trigger;
    public bool spawnEgglet;
    public int eggDistrict = 6;
    public AudioSource sound;
    Animator animator;
    Interactable interactable;
    MobAI eggletAI;

    private void Start()
    {
        animator = egg.GetComponent<Animator>();
        interactable = GetComponent<Interactable>();
    }

    private void Update()
    {
        if (spawnEgglet)
        {
            CheckEggletDead();
        }
    }
    public void Interacted()
    {
        animator.SetBool("isInteracted", true);
        Invoke("BreakEgg", 2);
        interactable.ClosePressE();
        trigger.enabled = false;
        sound.Play();
    }

    public void BreakEgg()
    {
        if (!breakFX.gameObject.activeSelf)
        {
            breakFX.gameObject.SetActive(true);
            breakFX.Play();
        }
        else
        {
            breakFX.Clear();
            breakFX.Play();
        }
        animator.SetBool("isInteracted", false);

        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0)
        {
            spawnEgglet = true;
            Invoke("EggletSpawn", 0.5f);
            trigger.enabled = false;

        }
        else
        {
            spawnEgglet = false;
            Invoke("EggRefresh", 5f);
            GetItemEgg();
        }
    }

    public void EggletSpawn()
    {
        egglet = MobPoolManager.instance.GetMob(MobType.egglet);
        eggletAI = egglet.GetComponent<MobAI>();
        eggletAI.currentDistrict = eggDistrict;
        eggletAI.Revive(transform.position);
        eggletAI.block.SetFloat("_Clipping_Level", 0.5f);
        eggletAI.m_bBattleBefore = true;
        egg.SetActive(false);
    }

    public void GetItemEgg()
    {
        int num = UnityEngine.Random.Range(1, 4);
        CanvasManager.instance.inventory.PutInBag(new ItemInformation(ItemName.egg, ItemManager.instance.GetItemSO(ItemName.egg), num));
        egg.SetActive(false);
    }

    public void EggRefresh()
    {
        Debug.Log("Egg Refresh");
        egg.SetActive(true);
        trigger.enabled = true;
        if (spawnEgglet)
        {
            spawnEgglet = false;
        }
    }

    public void CheckEggletDead()
    {
        if (eggletAI != null)
        {
            if (eggletAI.m_isDead)
            {
                Invoke("EggRefresh", 5f);
                egglet = null;
                eggletAI = null;
            }
        }

    }

}
