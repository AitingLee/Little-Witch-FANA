using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanel : MonoBehaviour
{
    public ItemName thisGem;
    public TextMeshProUGUI haveNumText;
    public Slider progressBar;
    public float progressTime;
    public GameObject successText, failText;
    public AudioSource successAudio, failAudio, upgradeAudio;

    Inventory inventory;
    int haveNum;
    bool startUpgrade;
    ItemSO gemSO;
    float timer;
    private void Update()
    {
        if (startUpgrade)
        {
            UpdateProgress();
        }
    }

    public void OpenPanel()
    {
        startUpgrade = false;
        CountHaveNum();
        successText.SetActive(false);
        failText.SetActive(false);
    }
    public void CountHaveNum()
    {
        inventory = CanvasManager.instance.inventory;
        foreach (Slot slot in inventory.slots)
        {
            if (slot.storeItem.itemInformation.itemName == thisGem)
            {
                haveNum = slot.storeItem.itemInformation.itemNumber;
                haveNumText.text = haveNum.ToString();
                return;
            }
        }
        haveNum = 0;
        haveNumText.text = haveNum.ToString();
    }
    public void OnUpgradeClick()
    {
        if (haveNum > 0)
        {
            foreach (Slot slot in inventory.slots)
            {
                if (slot.storeItem.itemInformation.itemName == thisGem)
                {
                    slot.storeItem.DecreaseAmount(1);
                    break;
                }
            }
            startUpgrade = true;
            timer = 0;
            upgradeAudio.Play();
            CountHaveNum();
        }
    }

    public void UpdateProgress()
    {
        timer += Time.deltaTime;
        if (timer <= progressTime)
        {
            progressBar.value = timer / progressTime;
        }
        else
        {
            startUpgrade = false;
            progressBar.value = 0;
            DoUpgrade();
        }
    }

    public void DoUpgrade()
    {
        int chance =  UnityEngine.Random.Range(0, 10);
        upgradeAudio.Stop();
        if (chance < 7)
        {
            //強化成功
            successText.SetActive(true);
            successAudio.Play();
            gemSO = ItemManager.instance.GetItemSO(thisGem);
            PlayerManager.instance.UpgradeGem(gemSO);
            Invoke("CloseSuccessText", 1f);
        }
        else
        {
            failText.SetActive(true);
            failAudio.Play();
            Invoke("CloseFailText", 1f);
        }
    }

    private void CloseSuccessText()
    {
        successText.SetActive(false);
    }

    private void CloseFailText()
    {
        failText.SetActive(false);
    }


}
