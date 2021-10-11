using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType { Gather, LightOrb, Tresure, Portal, Villager, ReviveRock, Totem, Egg }

public class Interactable : MonoBehaviour
{
    public InteractType type;
    public float refreshTime = 15f;
    public GameObject pressEGO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == InteractType.LightOrb)
            {
                LightOrb lightOrb = gameObject.GetComponent<LightOrb>();
                if (lightOrb != null)
                {
                    lightOrb.DisplayGetItemPanel();
                }
            }
            else if (type == InteractType.Tresure)
            {
                TreasureChest treasureChest = gameObject.GetComponent<TreasureChest>();
                if (treasureChest.isOpen)
                {
                    if (treasureChest.hasTreasure && treasureChest.chestItemInfos.Count >0)
                    {
                        treasureChest.DisplayGetItemPanel();
                    }
                    return;
                }
                else
                {
                    DisplayPressE();
                }
            }
            else
            {
                if (type == InteractType.Totem)
                {
                    Totem totem = gameObject.GetComponent<Totem>();
                    if (!totem.appeared)
                    {
                        return;
                    }
                }
                DisplayPressE();
            }
            InputManager.instance.E_Input = false;
            InputManager.instance.R_Input = false;
        }
    }

    private void DisplayPressE()
    {
        pressEGO = ObjectPoolManager.instance.GetPressE();
        PressE pressE = pressEGO.GetComponent<PressE>();
        pressE.followTarget = transform;
        pressEGO.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InputManager.instance.E_Input)
            {
                switch (type)
                {
                    case InteractType.Gather:
                        // TODO 存入背包
                        gameObject.SetActive(false);
                        Invoke("Refresh", refreshTime);
                        GatherItem gatherItem = gameObject.GetComponent<GatherItem>();
                        if (gatherItem != null)
                        {
                            gatherItem.GatherThisItem();
                        }
                        break;
                    case InteractType.LightOrb:
                        //檢取
                        Debug.Log("Press E Grab Item");
                        CanvasManager.instance.getItemPanel.GatherItemFromPanel();
                        break;
                    case InteractType.Tresure:
                        TreasureChest treasureChest = gameObject.GetComponent<TreasureChest>();
                        if (treasureChest == null)
                        {
                            return;
                        }
                        if (!treasureChest.isOpen)
                        {
                            treasureChest.OpenChest();
                            ClosePressE();
                        }
                        else
                        {
                            CanvasManager.instance.getItemPanel.GatherItemFromPanel();
                        }
                        break;
                    case InteractType.Portal:
                        TeleportPlatform teleportPlatform = gameObject.GetComponent<TeleportPlatform>();
                        if (teleportPlatform != null)
                        {
                            teleportPlatform.InteractWithPortal();
                        }
                        break;
                    case InteractType.Villager:
                        Villager villager = gameObject.GetComponent<Villager>();
                        if (villager != null)
                        {
                            villager.TriggerCurrentDialogue();
                            ClosePressE();
                        }
                        break;
                    case InteractType.ReviveRock:
                        RevivePoint revivePoint = gameObject.GetComponent<RevivePoint>();
                        if (revivePoint != null)
                        {
                            revivePoint.SetRecordPoint();
                            AudioManager.instance.reviveSound.Play();
                        }
                        break;
                    case InteractType.Totem:
                        Totem totem = gameObject.GetComponent<Totem>();
                        if (totem != null)
                        {
                            totem.TakeOrb();
                            if (totem.totemElement == TotemElement.earth)
                            {
                                TaskManager.instance.FinishTask(TaskManager.instance.allTasks[3]);
                            }
                        }
                        break;
                    case InteractType.Egg:
                        Egg egg = gameObject.GetComponent<Egg>();
                        if (egg != null)
                        {
                            egg.Interacted();
                        }
                        break;
                }
                InputManager.instance.E_Input = false;
            }

            if (InputManager.instance.R_Input)
            {
                switch (type)
                {
                    case InteractType.LightOrb:
                        //檢取
                        CanvasManager.instance.getItemPanel.SetPanelToNext();
                        break;
                    case InteractType.Tresure:
                        CanvasManager.instance.getItemPanel.SetPanelToNext();
                        break;
                    default:
                        break;
                }
                InputManager.instance.R_Input = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LeaveInteractable();
        }
    }

    public void ClosePressE()
    {
        if (pressEGO != null)
        {
            PressE pressE = pressEGO.GetComponent<PressE>();
            if (pressE != null)
            {
                pressE.followTarget = null;

            }
            pressEGO.SetActive(false);
            pressEGO = null;
        }
    }

    private void ItemRefresh()
    {
        gameObject.SetActive(true);
    }

    private void LeaveInteractable()
    {
        if (type == InteractType.LightOrb || type == InteractType.Tresure)
        {
            //關閉 Get Item Panel
            CanvasManager.instance.getItemPanel.Close();
        }
        else
        {
            //關閉 press E
            ClosePressE();
        }
    }

    private void OnDisable()
    {
        LeaveInteractable();
    }



}
