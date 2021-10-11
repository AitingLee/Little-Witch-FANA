using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VendorItemPanel : MonoBehaviour
{
    public ItemName itemName;
    public TextMeshProUGUI nameText, camptionText, numText, priceText;
    public Image image;

    ItemSO itemSO;
    int num, totalPrice;
    Inventory inventory;
    private void Start()
    {
        InitSellInfo();
        inventory = CanvasManager.instance.inventory;
    }
    private void InitSellInfo()
    {
        itemSO = ItemManager.instance.GetItemSO(itemName);
        image.sprite = itemSO.itemSprite;
        nameText.text = itemSO.itemNamestring;
        camptionText.text = itemSO.itemCaption;
        numText.text = "0";
        priceText.text = itemSO.buyPrice.ToString();
    }
    public void IncreaseNum()
    {
        if (num < 99)
        {
            num++;
        }
        else
        {
            return;
        }

        totalPrice = itemSO.buyPrice * num;

        if (num == 0)
        {
            priceText.text = itemSO.buyPrice.ToString();
        }
        else
        {
            priceText.text = totalPrice.ToString();
        }
        numText.text = num.ToString();
    }

    public void DecreaseNum()
    {
        if (num > 1)
        {
            num--;
        }
        else
        {
            return;
        }

        totalPrice = itemSO.buyPrice * num;

        if (num == 0)
        {
            priceText.text = itemSO.buyPrice.ToString();
        }
        else
        {
            priceText.text = totalPrice.ToString();
        }
        numText.text = num.ToString();

    }

    public void BuyItem()
    {
        if (inventory.cash > totalPrice)
        {
            if(inventory.PutInBag(new ItemInformation(itemName, itemSO, num)))
            {
                inventory.AddMoney(-totalPrice);
                AudioManager.instance.shopSound.Play();
                Debug.Log("¡ ∂R¶®•\");
            }
        }
    }

}
