using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    SaveData currentData;

    public Inventory inventory;

    private static SaveManager _instance;
    public static SaveManager instance
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


    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveGameHistory(int fileIndex)
    {
        SavePlayerData();
        SaveInventoryData();
        //先將current Data更新為最新遊戲進度再存成Json

        PlayerPrefs.SetString($"gameHistory{fileIndex}", JsonUtility.ToJson(currentData));
    }

    public void LoadGameHistory(int fileIndex)
    {
        //先讀取json為current Data再將將遊戲畫面更新為current Data的樣子
        currentData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString($"gameHistory{fileIndex}"));


        LoadPlayerData();
        LoadInventoryData();

    }

    [System.Serializable]
    public class SaveData
    {
        public int HP, maxHP, MP, maxMP;
        public int atk;
        public Vector3 position;
        public bool[] hasLearnSkill = new bool[4];      // 0 防護罩 1 火雨 2 龍捲風 3 冰錐

        public ItemName[] inventoryItemName = new ItemName[12];
        public int[] inventoryItemNum = new int[12];
        public int cash;
        public ItemName[] equippedItemName = new ItemName[3];     // 0 耳環 1 項鍊 2 戒指

        public bool[] portalIsActive = new bool[7];
        public bool[] chestIsOpen = new bool[12];
        public bool[] triggerHasEntered = new bool[5];
        public int missionProgress;
        public int reviveRecord;

        //TODO 開過的傳送門 寶箱(開過沒開過) 圖騰 怪物Trigger 任務進度 重生石 要幫以上幾個編碼
    }

    public void SavePlayerData()
    {
        currentData.HP = PlayerManager.instance.playerData.HP;
        currentData.maxHP = PlayerManager.instance.playerData.maxHP;
        currentData.MP = PlayerManager.instance.playerData.MP;
        currentData.maxMP = PlayerManager.instance.playerData.maxMP;
        currentData.atk = PlayerManager.instance.playerData.atk;
        currentData.position = PlayerManager.instance.transform.position;
    }

    public void LoadPlayerData()
    {
        PlayerManager.instance.playerData.HP = currentData.HP;
        PlayerManager.instance.playerData.maxHP = currentData.maxHP;
        PlayerManager.instance.playerData.MP = currentData.MP;
        PlayerManager.instance.playerData.maxMP = currentData.maxMP;
        PlayerManager.instance.playerData.atk = currentData.atk;
        PlayerManager.instance.transform.position = currentData.position;
    }

    public void SaveInventoryData()
    {
        inventory = CanvasManager.instance.inventory;

        for (int i = 0; i < 12; i++)
        {
            currentData.inventoryItemName[i] = inventory.slots[i].storeItem.itemInformation.itemName;
            currentData.inventoryItemNum[i] = inventory.slots[i].storeItem.itemInformation.itemNumber;
        }
        currentData.cash = inventory.cash;
    }


    private void LoadInventoryData()
    {
        inventory = CanvasManager.instance.inventory;

        //Data
        for (int i = 0; i < 12; i++)
        {
            inventory.slots[i].storeItem.itemInformation.itemName = currentData.inventoryItemName[i];
            inventory.slots[i].storeItem.itemInformation.itemNumber = currentData.inventoryItemNum[i];
        }

        inventory.cash = currentData.cash;



        //Display
        foreach (Slot slot in inventory.slots)
        {
            slot.storeItem.itemInformation.itemSO = ItemManager.instance.GetItemSO(slot.storeItem.itemInformation.itemName);
            slot.storeItem.UpdateInfo();
        }
        CanvasManager.instance.displayCashText.text = inventory.cash.ToString();

    }
}
