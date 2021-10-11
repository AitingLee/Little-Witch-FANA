using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Item2D : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public ItemInformation itemInformation;

    public Slot originalSlot;                               //�Ū��A�磌��parent��m��
    Transform mainCanvas;                                  //�Ū��A�磌��parent��m��(��Canvas)
    public Image thisImage;                                               //���~�Ϥ�
    public TextMeshProUGUI numText;

    void Start()
    {
        mainCanvas = CanvasManager.instance.transform;
    }


    public void OnBeginDrag(PointerEventData eventData)         //�Ԫ����
    {
        originalSlot = this.transform.parent.GetComponent<Slot>();
        if (originalSlot == null)
        {
            return;
        }
        if (originalSlot.slotZone == SlotZone.equipment)
        {
            return;
        }
        thisImage.raycastTarget = false;
        this.transform.SetParent(mainCanvas);
    }
    public void OnEndDrag(PointerEventData eventData)           //�����Ԫ���
    {

        if (this.transform.parent == mainCanvas.transform)
        {
            this.transform.SetParent(originalSlot.transform);           //�]�w����parent�b��
            this.transform.localPosition = Vector3.zero;            //�s��m�]�����ߨ쥿�T��m
        }
        thisImage.raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)              //�I����
    {
        if (originalSlot == null)
        {
            return;
        }
        this.transform.position = Input.mousePosition;          //���󬰹��ЦP��m
    }

    public void UpdateInfo()
    {
        Debug.Log($"UpdateInfo {itemInformation.itemName} num {itemInformation.itemNumber}");
        itemInformation.itemSO = ItemManager.instance.GetItemSO(itemInformation.itemName);
        thisImage.sprite = itemInformation.itemSO.itemSprite;
        if (itemInformation.itemNumber > 1)
        {
            numText.text = itemInformation.itemNumber.ToString();
        }
        else
        {
            numText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            originalSlot = this.transform.parent.GetComponent<Slot>();
            if (originalSlot == null)
            {
                return;
            }
            if (originalSlot.slotZone == SlotZone.inventory)
            {
                UseItem();
            }
            else if (originalSlot.slotZone == SlotZone.equipment)
            {
                UnEquipItem();
            }
        }
    }
    public void UseItem()
    {
        switch (itemInformation.itemSO.itemType)
        {
            case ItemType.consumable:
                Consume();
                DecreaseAmount(1);
                break;
            case ItemType.equipment:
                CanvasManager.instance.equipment.Equip(itemInformation);
                DecreaseAmount(1);
                break;
        }
    }

    public void UnEquipItem()
    {
        if (itemInformation.itemSO.itemType == ItemType.equipment)
        {
            Debug.Log($"should put in bag name{itemInformation.itemName}, SO name {itemInformation.itemSO.itemNamestring}, num {itemInformation.itemNumber}");
            if (CanvasManager.instance.inventory.PutInBag(new ItemInformation(itemInformation.itemName, itemInformation.itemSO, 1)))
            {
                PlayerManager.instance.EquipmentValueChange(-itemInformation.itemSO.increaseMaxHP, -itemInformation.itemSO.increaseMaxMP, -itemInformation.itemSO.increaseAttack);
                DecreaseAmount(1);
            }
        }
        CanvasManager.instance.equipment.UpdatePlayerData(PlayerManager.instance.playerData);
    }

    public void Consume()
    {
        Debug.Log($"heal hp {itemInformation.itemSO.healHp}");
        if (itemInformation.itemSO.healHp != 0)
        {
            PlayerManager.instance.AddHealthValue(itemInformation.itemSO.healHp);
        }
        if (itemInformation.itemSO.healMp != 0)
        {
            PlayerManager.instance.AddManaValue(itemInformation.itemSO.healMp);
        }

    }

    public void IncreaseAmount(int amount)
    {
        itemInformation.itemNumber += amount;
        UpdateInfo();
    }

    public void DecreaseAmount(int howMuch)
    {
        Debug.Log($"this item num = {itemInformation.itemNumber}, decrease amount = {howMuch}");
        if (howMuch > itemInformation.itemNumber)
        {
            Debug.Log("Count Error, can't take so much");
        }
        else if (howMuch == itemInformation.itemNumber)
        {
            itemInformation.itemName = ItemName.empty;
            itemInformation.itemSO = ItemManager.instance.GetItemSO(ItemName.empty);
            itemInformation.itemNumber = 0;
        }
        else
        {
            itemInformation.itemNumber -= howMuch;
        }
        UpdateInfo();
    }
}
