using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    //§Úªº
    private static CanvasManager _instance;
    public static CanvasManager instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    [Header("Aim Mode")]
    public GameObject aimScreen;

    [Header("Tab Panel")]
    public TabPanel tabPanel;
    public GameObject playerInfos;
    public TextMeshProUGUI midPanelText;
    public GameObject worldMap;

    public SkillPanel skillPanel;
    public SettingPanel settingPanel;

    [Header("Inventory")]
    public Inventory inventory;
    public TextMeshProUGUI displayItemName;
    public TextMeshProUGUI displayItemCaption;
    public TextMeshProUGUI displayCashText;

    [Header("Equipment")]
    public Equipment equipment;


    [Header("Shop System")]
    public ShopSystem shopSystem;
    public SelectNumPanel selectNumPanel;

    [Header("Get Item")]
    [SerializeField]public GetItemPanel getItemPanel;

    [Header("Teleport")]
    public GameObject teleportScreen;
    public TeleportButton[] teleportButtons;
    public List<TeleportPlatform> allPlateforms;

    [Header("Cursor")]
    public Texture2D cursorTexture;
    public bool freezeTime;

    [Header("Boss")]
    public GameObject bossHealthBar;
    public bool showPanel;

    private void Awake()
    {
        _instance = this;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void Start()
    {
        displayItemName.text = "";
        displayItemCaption.text = "";
        displayCashText.text = "0";        
    }

    public void DisplayAimScreen()
    {
        aimScreen.SetActive(true);
    }

    public void HideAimScreen()
    {
        aimScreen.SetActive(false);
    }


    public void PressTab()
    {
        if (tabPanel.isOpen)
        {
            tabPanel.Close();
            PlayerManager.instance.DefreezeTimeScale();
            equipment.CloseEquipmentPanel();
            skillPanel.gameObject.SetActive(false);
            settingPanel.gameObject.SetActive(false);
        }
        else
        {
            tabPanel.Display();
            PlayerManager.instance.FreezeTimeScale();
            inventory.DisplayInventory();
            equipment.CloseEquipmentPanel();
            skillPanel.gameObject.SetActive(false);
            settingPanel.gameObject.SetActive(false);
        }
    }

    public void VendorIsOpened()
    {
        tabPanel.Display();
        PlayerManager.instance.FreezeTimeScale();
        inventory.DisplayInventory();
        equipment.CloseEquipmentPanel();
    }

    public void VendorIsClosed()
    {
        tabPanel.Close();
        PlayerManager.instance.DefreezeTimeScale();
    }

    public void ShowCuresor()
    {
        showPanel = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        InputManager.instance.ClearAllInput();
        freezeTime = true;
 
    }

    public void HideCuresor()
    {
        showPanel = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        InputManager.instance.ClearAllInput();
        freezeTime = false;
    }

    public void DiaplayPlayerInfo(bool display)
    {
        if (display)
        {
            playerInfos.SetActive(true);
        }
        else
        {
            playerInfos.SetActive(false);
        }
    }

    public void DisplayMap()
    {
        if (worldMap.activeSelf)
        {
            worldMap.SetActive(false);
            PlayerManager.instance.DefreezeTimeScale();
        }
        else
        {
            worldMap.SetActive(true);
            PlayerManager.instance.FreezeTimeScale();
        }
    }

}
