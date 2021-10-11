using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public List<ItemInformation> chestItemInfos;
    public Animator animator;
    public GameObject lightFX, innerObject;
    public bool hasTreasure, isOpen;
    public AudioSource sound;
    private void Start()
    {
        hasTreasure = true;
        isOpen = false;
    }

    public void DisplayGetItemPanel()
    {
        CanvasManager.instance.getItemPanel.Display(chestItemInfos, this);
    }

    public void OpenChest()
    {
        animator.SetBool("isOpen", true);
        lightFX.SetActive(true);
        isOpen = true;
        DisplayGetItemPanel();
        sound.Play();
    }

    public void EmptyChest()
    {
        lightFX.GetComponent<ParticleSystem>().Clear();
        lightFX.SetActive(false);
        chestItemInfos = null;
        innerObject.SetActive(false);
        hasTreasure = false;
    }

}
