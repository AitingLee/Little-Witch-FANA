using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotZone {inventory, equipment, sell, getPanel}

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public SlotZone slotZone;
    public Item2D storeItem;

    public void Awake()
    {
        storeItem = transform.GetChild(0).GetComponent<Item2D>();
    }
    public void Start()
    {
        storeItem.itemInformation.itemSO = ItemManager.instance.GetItemSO(ItemName.empty);
        storeItem.itemInformation.itemNumber = 0;
        storeItem.UpdateInfo();
    }

    public void PutInEmptySlot(ItemInformation putInfo)
    {
        storeItem.itemInformation = putInfo;
        storeItem.UpdateInfo();
    }

    public void OnPointerEnter(PointerEventData eventData)              //��ƹ��b�W��
    {
        if (slotZone == SlotZone.inventory)
        {
            //��ܪ��~��T-->�}�T������
            CanvasManager.instance.displayItemCaption.text = storeItem.itemInformation.itemSO.itemCaption;
            CanvasManager.instance.displayItemName.text = storeItem.itemInformation.itemSO.itemNamestring;
        }

    }

    public void OnPointerExit(PointerEventData eventData)               //��ƹ��S�b�W��
    {
        if (slotZone == SlotZone.inventory)
        {
            //����ܪ��~��T-->���T������
            CanvasManager.instance.displayItemCaption.text = "";
            CanvasManager.instance.displayItemName.text = "";
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
        GameObject dragObject = eventData.pointerDrag;

        if (slotZone == SlotZone.inventory)     //���I�]
        {
            Transform originalItem = storeItem.transform;
            Slot dragFromSlot = dragObject.GetComponent<Item2D>().originalSlot;
            if (dragFromSlot.slotZone == SlotZone.inventory)        //�I�]���I�]
            {
                if (originalItem != null)
                {
                    originalItem.SetParent(dragFromSlot.transform); //�洫�����m
                    originalItem.localPosition = Vector3.zero;                                       //�s��m�]�����ߨ쥿�T��m
                    dragFromSlot.storeItem = storeItem;
                }
                dragObject.transform.SetParent(transform);
                dragObject.transform.localPosition = Vector3.zero;
                storeItem = dragObject.GetComponent<Item2D>();
            }
            else if (dragFromSlot.slotZone == SlotZone.sell)        //�c����I�]
            {
                if (CanvasManager.instance.inventory.PutInBag(dragObject.GetComponent<Item2D>().itemInformation))
                {
                    dragFromSlot.storeItem.itemInformation = new ItemInformation(ItemName.empty, ItemManager.instance.GetItemSO(ItemName.empty), 0);
                    dragFromSlot.storeItem.UpdateInfo();
                }
            }

        }
        else if (slotZone == SlotZone.sell)     //���c��
        {
            Slot dragFromSlot = dragObject.GetComponent<Item2D>().originalSlot;
            if (dragFromSlot.slotZone == SlotZone.inventory)        //�I�]���c��
            {
                CanvasManager.instance.selectNumPanel.dropToSell(this, dragObject.GetComponent<Item2D>());
            }
        }
    }

}
