using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] slots;
    public int cash;
    public GameObject[] bagPage;

    void Start()
    {
        bagPage[1].SetActive(false);
        bagPage[2].SetActive(false);
    }

    public bool PutInBag(ItemInformation item)
    {
        if (item.itemName == ItemName.money)
        {
            AddMoney(item.itemNumber);
            return true;
        }

        foreach (Slot slot in slots)
        {
            if (slot.storeItem.itemInformation.itemName != ItemName.empty)
            {
                if (slot.storeItem.itemInformation.itemName == item.itemName)
                {
                    slot.storeItem.IncreaseAmount(item.itemNumber);
                    slot.storeItem.UpdateInfo();
                    return true;
                }
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.storeItem.itemInformation.itemName == ItemName.empty)
            {
                Debug.Log($"put in bag's empty slot name{item.itemName}, SO name {item.itemSO.itemNamestring}, num {item.itemNumber}");
                slot.PutInEmptySlot(item);
                return true;
            }
        }

        return false;
    }

    public void AddMoney(int howMuch)
    {
        if (howMuch > 0)
        {
            cash += howMuch;
        }
        if (howMuch < 0)
        {
            if (cash + howMuch >= 0)
            {
                cash += howMuch;
            }
            else
            {
                cash = 0;
            }
        }
        CanvasManager.instance.displayCashText.text = cash.ToString();
    }

    public void DisplayInventory()
    {
        gameObject.SetActive(true);
        CanvasManager.instance.midPanelText.text = "­I¥]";
    }

}
