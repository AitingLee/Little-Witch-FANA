using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherItem : MonoBehaviour
{
    public ItemInformation thisItem;
    
    public void GatherThisItem()
    {
        CanvasManager.instance.inventory.PutInBag(thisItem);
    }
}
