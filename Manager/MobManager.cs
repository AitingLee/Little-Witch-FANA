using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MobType { seed, sprout, flower, egglet, chick, fledgling, bird, spider, spiderDark, spiderToxin, bee, bumblePale, bumbleYello, sting, bat, vampire, batLord, spooky, ghost, phantom, dragon };

public class MobManager : MonoBehaviour
{
    public static MobManager instance;

    public List<GameObject> m_aliveMobs;
    private List<Obstacle> m_Obstacles;

    public GameObject m_Player;
    public LayerMask m_groundLayer;

    [Header("Path Point")]
    public Transform[] districtPointsParent;
    public List<PathPoint>[] districtPoints;

    [Header("Mob On Scene")]
    [SerializeField] public List<GameObject>[] districtMobs;
    public float[] districtMobAmount;
    public CapsuleCollider dragonCollider;


    private void Awake()
    {
        Debug.Log("Mob Manager Awake");
        instance = this;


        districtMobs = new List<GameObject>[districtMobAmount.Length];
        for (int i = 0; i < districtMobs.Length; i++)
        {
            districtMobs[i] = new List<GameObject>();
        }
        InitMobs();
        InitObstacles();
        InitPathPoints();

    }
    private void Start()
    {
        m_Player = PlayerManager.instance.transform.gameObject;
        m_groundLayer = m_Player.GetComponent<PlayerLocomotion>().groundLayer;
    }

    void InitMobs()
    {
        m_aliveMobs = new List<GameObject>();
        GameObject[] mobsGO = GameObject.FindGameObjectsWithTag("Mob");
        GameObject[] eliteGO = GameObject.FindGameObjectsWithTag("Elite");
        if (mobsGO != null || mobsGO.Length > 0)
        {
            foreach (GameObject go in mobsGO)
            {
                if (go.activeSelf)
                {
                    m_aliveMobs.Add(go);
                }
            }
        }
        if (eliteGO != null || eliteGO.Length > 0)
        {
            foreach (GameObject go in eliteGO)
            {
                if (go.activeSelf)
                {
                    m_aliveMobs.Add(go);
                }
            }
        }
    }

    void InitObstacles()
    {
        m_Obstacles = new List<Obstacle>();
        GameObject[] obsGO = GameObject.FindGameObjectsWithTag("Obstacles");
        Debug.Log($"count obstacle = {obsGO.Length}");
        if (obsGO != null || obsGO.Length > 0)
        {
            foreach (GameObject go in obsGO)
            {
                if (go.GetComponent<Obstacle>() != null)
                {
                    m_Obstacles.Add(go.GetComponent<Obstacle>());
                }
                else
                {
                    Debug.Log($"obstacle {go.name} don't have script");
                }
            }
        }
    }

    void InitPathPoints()
    {
        int distCount = districtPointsParent.Length;
        districtPoints = new List<PathPoint>[distCount];
        for (int i = 0; i < distCount; i++)
        {
            districtPoints[i] = new List<PathPoint>();
        }

        for (int i = 0; i < distCount; i++)
        {
            for (int j = 0; j < districtPointsParent[i].childCount; j++)
            {
                PathPoint pp = districtPointsParent[i].GetChild(j).GetComponent<PathPoint>();
                if (pp != null)
                {
                    districtPoints[i].Add(pp);
                }
                else
                {
                    Debug.Log("pp not found");
                }
            }
        }
    }

    public GameObject GetPlayer()
    {
        return m_Player;
    }

    public List<Obstacle> GetObstacles()
    {
        return m_Obstacles;
    }

    public List<GameObject> GetMobs()
    {
        return m_aliveMobs;
    }

    public List<PathPoint> GetDistrictPoints(int district)
    {
        return districtPoints[district];
    }

    public void AddDistrictMob(int district, GameObject mobGO)
    {
        if (mobGO.tag == "Mob")
        {
            districtMobs[district].Add(mobGO);
        }
    }

    public void RemoveDistrictMob(int district, GameObject mobGO)
    {
        Debug.Log($"{mobGO.name} is removing from {district} district");
        if (mobGO.tag == "Mob")
        {
            districtMobs[district].Remove(mobGO);
            float shortage = districtMobs[district].Count - districtMobAmount[district];
            Debug.Log($"shortage = {shortage}");
            if (shortage < 0)
            {
                for (int i = 0; i < -shortage; i++)
                {
                    MobAI getMob = RandomMobFromPool(district);
                    getMob.Spawn(district);
                }
            }
        }
    }

    private MobAI RandomMobFromPool(int district)
    {
        MobAI getMob = null;
        GameObject mobGO = null;
        int rand;

        switch (district)
        {
            case 0:
                rand = UnityEngine.Random.Range(0, 5);
                if (rand < 3)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.seed);
                }
                else
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.sprout);
                }
                break;
            case 1:
                rand = UnityEngine.Random.Range(0, 10);
                if (rand < 2)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.seed);
                }
                else if (rand < 4)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.sprout);
                }
                else if (rand < 7)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spider);
                }
                else
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spiderDark);
                }
                break;
            case 2:
                rand = UnityEngine.Random.Range(0, 4);
                if (rand < 1)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.seed);
                }
                else if (rand < 2)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.sprout);
                }
                else if (rand < 3)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spider);
                }
                else
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spiderDark);
                }
                break;
            case 3:
                rand = UnityEngine.Random.Range(0, 10);
                if (rand < 2)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spider);
                }
                else if (rand < 4)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spiderDark);
                }
                else if (rand < 7)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.bat);
                }
                else
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.vampire);
                }
                break;
            case 4:
                rand = UnityEngine.Random.Range(0, 10);
                if (rand < 2)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.seed);
                }
                else if (rand < 4)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.sprout);
                }
                else if (rand < 7)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spider);
                }
                else
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spiderDark);
                }
                break;
            case 5:
                rand = UnityEngine.Random.Range(0, 4);
                if (rand < 1)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.bat);
                }
                else if (rand < 2)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.vampire);
                }
                else if (rand < 3)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spider);
                }
                else
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spiderDark);
                }
                break;
            case 6:
                rand = UnityEngine.Random.Range(0, 6);
                if (rand < 2)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.chick);
                }
                else if (rand < 4)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.fledgling);
                }
                else if (rand < 5)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.bumblePale);
                }
                else
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.bumbleYello);
                }
                break;
            case 7:
                rand = UnityEngine.Random.Range(0, 4);
                if (rand < 1)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.bumblePale);
                }
                else if (rand < 2)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.bumbleYello);
                }
                else if (rand < 3)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.chick);
                }
                else
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.fledgling);
                }
                break;
            case 8:
                rand = UnityEngine.Random.Range(0, 7);
                if (rand < 4)
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.spooky);
                }
                else
                {
                    mobGO = MobPoolManager.instance.GetMob(MobType.ghost);
                }
                break;
        }

        getMob = mobGO.GetComponent<MobAI>();

        return getMob;
    }


    public List<ItemInformation> RandonDropList(MobType mobtype)
    {
        List<ItemInformation> dropItems = new List<ItemInformation>();
        int dropCount = 0;
        int dropMoney = 5;

        switch (mobtype)
        {
            case MobType.seed:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.weeds, ItemName.flower, ItemName.apple, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(5, 10);
                    Debug.Log($"seed drop count {i} : name = {randonName.ToString()}");
                }
                break;
            case MobType.sprout:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.weeds, ItemName.flower, ItemName.apple, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(8, 12);
                }
                break;
            case MobType.flower:
                dropCount = RandomEliteDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomEliteDropItemName(ItemName.weeds, ItemName.flower, ItemName.apple, ItemName.apple, ItemName.silverNecklace, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(30, 50);
                }
                break;
            case MobType.egglet:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.weeds, ItemName.egg, ItemName.meat, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(2, 5);
                }
                break;
            case MobType.chick:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.weeds, ItemName.meat, ItemName.roastChicken, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(8, 12);
                }
                break;
            case MobType.fledgling:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.flower, ItemName.meat, ItemName.roastChicken, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(10, 15);
                }
                break;
            case MobType.bird:
                dropCount = RandomEliteDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomEliteDropItemName(ItemName.egg, ItemName.meat, ItemName.roastChicken, ItemName.roastChicken, ItemName.silverEarrings, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(40, 60);
                }
                break;
            case MobType.spider:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.silk, ItemName.rock, ItemName.carrot, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(12, 18);
                }
                break;
            case MobType.spiderDark:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.silk, ItemName.rock, ItemName.carrot, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(12, 18);
                }
                break;
            case MobType.spiderToxin:
                dropCount = RandomEliteDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomEliteDropItemName(ItemName.silk, ItemName.rock, ItemName.carrot, ItemName.carrot, ItemName.silverRing, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(50, 70);
                }
                break;
            case MobType.bee:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.jar, ItemName.wood, ItemName.honey, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(15, 20);
                }
                break;
            case MobType.bumblePale:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.jar, ItemName.wood, ItemName.honey, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(20, 25);
                }
                break;
            case MobType.bumbleYello:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.jar, ItemName.wood, ItemName.honey, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(20, 25);
                }
                break;
            case MobType.sting:
                dropCount = RandomEliteDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomEliteDropItemName(ItemName.jar, ItemName.wood, ItemName.honey, ItemName.moonStone, ItemName.agateNecklace, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(60, 80);
                }
                break;
            case MobType.bat:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.wood, ItemName.rock, ItemName.broccoli, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(20, 25);
                }
                break;
            case MobType.vampire:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.wood, ItemName.rock, ItemName.broccoli, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(25, 30);
                }
                break;
            case MobType.batLord:
                dropCount = RandomEliteDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomEliteDropItemName(ItemName.wood, ItemName.rock, ItemName.broccoli, ItemName.amethyst, ItemName.rubyRing, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(70, 90);
                }
                break;
            case MobType.spooky:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.ice, ItemName.popsicle, ItemName.shavedIce, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(28, 35);
                }
                break;
            case MobType.ghost:
                dropCount = RandomMobDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomMobDropItemName(ItemName.ice, ItemName.popsicle, ItemName.shavedIce, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(32, 40);
                }
                break;
            case MobType.phantom:
                dropCount = RandomEliteDropCount();
                for (int i = 0; i < dropCount; i++)
                {
                    int randonNumber = 0;
                    ItemName randonName = RandomEliteDropItemName(ItemName.ice, ItemName.popsicle, ItemName.shavedIce, ItemName.cryolite, ItemName.jadeEarrings, ref randonNumber);
                    dropItems.Add(new ItemInformation(randonName, ItemManager.instance.GetItemSO(randonName), randonNumber));
                    dropMoney = UnityEngine.Random.Range(80, 100);
                }
                break;
        }

        //RandomMoney
        dropItems.Add(new ItemInformation(ItemName.money, ItemManager.instance.GetItemSO(ItemName.money), dropMoney));
        return dropItems;
    }

    public int RandomMobDropCount()
    {
        //獲取小怪掉落物品數量
        int count = 0;
        int rand = UnityEngine.Random.Range(1, 11);
        if (rand < 8)
        {
            count = 1;
        }
        else if (rand < 10)
        {
            count = 2;
        }
        else if (rand == 10)
        {
            count = 3;
        }
        return count;
    }

    public ItemName RandomMobDropItemName(ItemName name1rate40, ItemName name2rate40, ItemName name3rate20, ref int number)
    {
        int rand = UnityEngine.Random.Range(1, 11);
        if (rand < 5)
        {
            number = UnityEngine.Random.Range(1, 6);
            return name1rate40;
        }
        else if (rand < 9)
        {
            number = UnityEngine.Random.Range(1, 6);
            return name2rate40;
        }
        else if (rand < 11)
        {
            number = UnityEngine.Random.Range(1, 3);
            return name3rate20;
        }
        return ItemName.empty;
    }

    public int RandomEliteDropCount()
    {
        //獲取菁英怪掉落物品數量
        int count = 0;
        int rand = UnityEngine.Random.Range(1, 11);
        if (rand < 3)
        {
            count = 2;
        }
        else if (rand < 9)
        {
            count = 3;
        }
        else if (rand < 11)
        {
            count = 4;
        }
        return count;
    }

    public ItemName RandomEliteDropItemName(ItemName name1rate37, ItemName name2rate37, ItemName name3rate18, ItemName name4rate5, ItemName name5rate3, ref int number)
    {
        int rand = UnityEngine.Random.Range(1, 101);
        if (rand < 38)
        {
            number = UnityEngine.Random.Range(1, 6);
            return name1rate37;
        }
        else if (rand < 75)
        {
            number = UnityEngine.Random.Range(1, 6);
            return name2rate37;
        }
        else if (rand < 94)
        {
            number = UnityEngine.Random.Range(1, 3);
            return name3rate18;
        }
        else if (rand < 98)
        {
            number = 1;
            return name4rate5;
        }
        else if (rand < 101)
        {
            number = 1;
            return name5rate3;
        }
        return ItemName.empty;
    }


}