using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInformation
{
    public ItemName itemName;
    public ItemSO itemSO;
    public int itemNumber;

    public ItemInformation(ItemName setName, ItemSO setSO, int setNum)
    {
        itemName = setName;
        itemSO = setSO;
        itemNumber = setNum;
    }
}
