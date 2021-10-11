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

    public void OnPointerEnter(PointerEventData eventData)              //當滑鼠在上面
    {
        if (slotZone == SlotZone.inventory)
        {
            //顯示物品資訊-->開訊息介面
            CanvasManager.instance.displayItemCaption.text = storeItem.itemInformation.itemSO.itemCaption;
            CanvasManager.instance.displayItemName.text = storeItem.itemInformation.itemSO.itemNamestring;
        }

    }

    public void OnPointerExit(PointerEventData eventData)               //當滑鼠沒在上面
    {
        if (slotZone == SlotZone.inventory)
        {
            //不顯示物品資訊-->關訊息介面
            CanvasManager.instance.displayItemCaption.text = "";
            CanvasManager.instance.displayItemName.text = "";
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
        GameObject dragObject = eventData.pointerDrag;

        if (slotZone == SlotZone.inventory)     //拖到背包
        {
            Transform originalItem = storeItem.transform;
            Slot dragFromSlot = dragObject.GetComponent<Item2D>().originalSlot;
            if (dragFromSlot.slotZone == SlotZone.inventory)        //背包拖到背包
            {
                if (originalItem != null)
                {
                    originalItem.SetParent(dragFromSlot.transform); //交換物件位置
                    originalItem.localPosition = Vector3.zero;                                       //新位置設為中心到正確位置
                    dragFromSlot.storeItem = storeItem;
                }
                dragObject.transform.SetParent(transform);
                dragObject.transform.localPosition = Vector3.zero;
                storeItem = dragObject.GetComponent<Item2D>();
            }
            else if (dragFromSlot.slotZone == SlotZone.sell)        //販賣拖到背包
            {
                if (CanvasManager.instance.inventory.PutInBag(dragObject.GetComponent<Item2D>().itemInformation))
                {
                    dragFromSlot.storeItem.itemInformation = new ItemInformation(ItemName.empty, ItemManager.instance.GetItemSO(ItemName.empty), 0);
                    dragFromSlot.storeItem.UpdateInfo();
                }
            }

        }
        else if (slotZone == SlotZone.sell)     //拖到販賣
        {
            Slot dragFromSlot = dragObject.GetComponent<Item2D>().originalSlot;
            if (dragFromSlot.slotZone == SlotZone.inventory)        //背包拖到販賣
            {
                CanvasManager.instance.selectNumPanel.dropToSell(this, dragObject.GetComponent<Item2D>());
            }
        }
    }

}
