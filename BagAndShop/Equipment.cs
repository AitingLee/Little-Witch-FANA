using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Equipment : MonoBehaviour
{
    public Slot earningsSlot, neckLaceSlot, ringSlot;
    public Transform leftParent, midParent;
    public TextMeshProUGUI HPtext, MPtext, atkText;

    //由item 2D 呼叫
    public void Equip(ItemInformation item)
    {
        if (item.itemSO.itemType != ItemType.equipment)
        {
            return;
        }
        else
        {
            if (item.itemName == ItemName.jadeEarrings || item.itemName == ItemName.silverEarrings)
            {
                EquipAtSlot(earningsSlot, item);
            }
            else if (item.itemName == ItemName.agateNecklace || item.itemName == ItemName.silverNecklace)
            {
                EquipAtSlot(neckLaceSlot, item);
            }
            else if (item.itemName == ItemName.rubyRing || item.itemName == ItemName.silverRing)
            {
                EquipAtSlot(ringSlot, item);
            }
        }
    }

    public void EquipAtSlot(Slot partSlot, ItemInformation item)
    {
        DisplayEquipmentPanel(false);
        ItemInformation originalItem = partSlot.storeItem.itemInformation;

        if (originalItem.itemName != ItemName.empty)
        {
            if (!CanvasManager.instance.inventory.PutInBag(originalItem))
            { 
                return;
            }
        }
        int HPgap = item.itemSO.increaseMaxHP - originalItem.itemSO.increaseMaxHP;
        int MPgap = item.itemSO.increaseMaxMP - originalItem.itemSO.increaseMaxMP;
        int atcGap = item.itemSO.increaseAttack - originalItem.itemSO.increaseAttack;

        PlayerManager.instance.EquipmentValueChange(HPgap, MPgap, atcGap);

        partSlot.PutInEmptySlot(new ItemInformation(item.itemName, item.itemSO, 1));
        partSlot.storeItem.UpdateInfo();
        CanvasManager.instance.equipment.UpdatePlayerData(PlayerManager.instance.playerData);
    }
    public void DisplayEquipmentPanel(bool atMid)
    {
        if (atMid)
        {
            transform.SetParent(midParent);
            transform.localPosition = Vector3.zero;
            if (!midParent.gameObject.activeSelf)
            {
                midParent.gameObject.SetActive(true);
            }
        }
        else
        {
            transform.SetParent(leftParent);
            transform.localPosition = Vector3.zero;
            if (!leftParent.gameObject.activeSelf)
            {
                leftParent.gameObject.SetActive(true);
            }
        }
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

    }
    public void CloseEquipmentPanel()
    {
        transform.SetParent(midParent);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
        if (leftParent.gameObject.activeSelf)
        {
            leftParent.gameObject.SetActive(false);
        }
    }

    public void UpdatePlayerData(PlayerData data)
    {
        int equipmentAddHP = neckLaceSlot.storeItem.itemInformation.itemSO.increaseMaxHP;
        int equipmentAddMP = earningsSlot.storeItem.itemInformation.itemSO.increaseMaxHP;
        int equipmentAddATK = ringSlot.storeItem.itemInformation.itemSO.increaseMaxHP;
        if (equipmentAddHP > 0)
        {
            HPtext.text = $"生命值上限：{data.maxHP - equipmentAddHP}（+{equipmentAddHP}）";
        }
        else
        {
            HPtext.text = $"生命值上限：{data.maxHP}";
        }

        if (equipmentAddMP > 0)
        {
            MPtext.text = $"魔力值上限：{data.maxMP - equipmentAddMP}（+{equipmentAddMP}）";
        }
        else
        {
            MPtext.text = $"魔力值上限：{data.maxMP}";
        }

        if (equipmentAddATK > 0)
        {
            atkText.text = $"攻擊力：{data.atk - equipmentAddATK}（+{equipmentAddATK}）";
        }
        else
        {
            atkText.text = $"攻擊力：{data.atk}";
        }
    }
    

}
