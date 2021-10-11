using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    public CharacterName owner;
    public Animator animator;
    public  PlayerSellPanel playerSell;

    public UpgradePanel[] upgradePanels;
    public void OpenVendor()
    {
        animator.SetBool("isOpen", true);
        if (upgradePanels.Length > 0)
        {
            foreach (UpgradePanel panel in upgradePanels)
            {
                panel.OpenPanel();
            }
        }
    }

    public void CloseVendor()
    {
        animator.SetBool("isOpen", false);
        if (playerSell != null)
        {
            playerSell.ClosePanel();
        }
    }

    public void OnCloseButtonClick()
    {
        CloseVendor();
        CanvasManager.instance.VendorIsClosed();
    }
}
