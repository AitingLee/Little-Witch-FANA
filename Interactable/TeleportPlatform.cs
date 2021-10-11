using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TeleportArea { Area1, Area2, Area3, Area4, Area5, Area6, Area7}

public class TeleportPlatform : MonoBehaviour
{
    public bool isActived;
    public GameObject portalFX;
    public TeleportArea platformArea;
    public GameObject activedIcon, activedMini;
    public void InteractWithPortal()
    {
        if (isActived)
        {
            DisplayTeleportScreen();
        }
        else
        {
            ActivePortal();
        }
    }

    public void ActivePortal()
    {
        portalFX.SetActive(true);
        isActived = true;
        Invoke("DisplayTeleportScreen", 3f);
        activedIcon.SetActive(false);
        activedMini.SetActive(false);
        AudioManager.instance.portalSound.Play();
    }


    public void DisplayTeleportScreen()
    {
        foreach (TeleportButton tb in CanvasManager.instance.teleportButtons)
        {
            if (tb.buttonArea == platformArea)
            {
                tb.gameObject.SetActive(false);
            }
            else
            {
                foreach (TeleportPlatform tp in CanvasManager.instance.allPlateforms)
                {
                    if (tp.platformArea == tb.buttonArea)
                    {
                        if (tp.isActived)
                        {
                            tb.gameObject.SetActive(true);
                        }
                        else
                        {
                            tb.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        CanvasManager.instance.teleportScreen.SetActive(true);
        CanvasManager.instance.ShowCuresor();
    }

    public void HideTeleportScreen()
    {
        CanvasManager.instance.teleportScreen.SetActive(false);
        CanvasManager.instance.HideCuresor();
    }

    public void TeleportExitButtonOnClick()
    {
        HideTeleportScreen();
    }

}
