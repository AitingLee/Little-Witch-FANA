using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public Vendor[] vendors;
    public void OpenCertainVendor(CharacterName talkTo)
    {
        foreach (Vendor vendor in vendors)
        {
            if (vendor.owner == talkTo)
            {
                vendor.OpenVendor();
            }
            else
            {
                vendor.CloseVendor();
            }
        }
        CanvasManager.instance.VendorIsOpened();
    }

}
