using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectNumPanel : MonoBehaviour
{
    Slot targetSellSlot;
    Item2D targetSellItem;
    public TextMeshProUGUI currentNumText;
    int currentNum;

    public void dropToSell(Slot playerSellSlot, Item2D sellItem)
    {
        gameObject.SetActive(true);
        targetSellSlot = playerSellSlot;
        targetSellItem = sellItem;
        currentNum = 0;
        currentNumText.text = "0";
    }

    public void IncreaseNum()
    {
        if (currentNum < targetSellItem.itemInformation.itemNumber)
        {
            currentNum++;
        }
        else
        {
            return;
        }
        currentNumText.text = currentNum.ToString();
    }
    public void DecreaseNum()
    {
        if (currentNum > 0)
        {
            currentNum--;
        }
        else
        {
            return;
        }
        currentNumText.text = currentNum.ToString();
    }

    public void OnConfirmClick()
    {
        if (targetSellSlot.storeItem.itemInformation.itemName == ItemName.empty)
        {
            targetSellSlot.PutInEmptySlot(new ItemInformation(targetSellItem.itemInformation.itemName, targetSellItem.itemInformation.itemSO, currentNum));
            targetSellItem.DecreaseAmount(currentNum);
        }
        gameObject.SetActive(false);
    }

    public void OnCancelClick()
    {
        gameObject.SetActive(false);
        targetSellSlot = null;
        targetSellItem = null;
    }

}
