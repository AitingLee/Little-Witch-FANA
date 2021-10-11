using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSellPanel : MonoBehaviour
{
    public Slot[] slots;
    public TextMeshProUGUI getPriceText;
    public int getPrice;

    public void Update()
    {
        UpdatePrice();
    }

    public void UpdatePrice()
    {
        int price = 0;
        foreach (Slot slot in slots)
        {
            price += slot.storeItem.itemInformation.itemNumber * slot.storeItem.itemInformation.itemSO.sellPrice;
        }
        getPrice = price;
        getPriceText.text = getPrice.ToString();
    }

    public void ClosePanel()
    {
        foreach (Slot slot in slots)
        {
            if (slot.storeItem.itemInformation.itemName != ItemName.empty)
            {
                CanvasManager.instance.inventory.PutInBag(slot.storeItem.itemInformation);
            }
        }
    }

    public void OnSellClick()
    {
        CanvasManager.instance.inventory.AddMoney(getPrice);
        AudioManager.instance.shopSound.Play();
        foreach (Slot slot in slots)
        {
            slot.storeItem.itemInformation = new ItemInformation(ItemName.empty, ItemManager.instance.GetItemSO(ItemName.empty), 0);
            slot.storeItem.UpdateInfo();
        }
    }
}
