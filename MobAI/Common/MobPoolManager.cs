using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MobPoolManager : MonoBehaviour
{
    private static MobPoolManager _instance;

    public static MobPoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MobPoolManager();
            }

            return _instance;
        }
    }
    [Header("Seed Pool")]
    [SerializeField] public Transform m_seedParent;
    [SerializeField] private GameObject m_seedPrefab;
    [SerializeField] private int m_seedPoolSize = 5;
    [SerializeField] public List<GameObject> m_seedPool;

    [Header("Sprout Pool")]
    [SerializeField] public Transform m_sproutParent;
    [SerializeField] private GameObject m_sproutPrefab;
    [SerializeField] private int m_sproutPoolSize = 5;
    [SerializeField] public List<GameObject> m_sproutPool;

    [Header("Flower Pool")]
    [SerializeField] public Transform m_flowerParent;
    [SerializeField] private GameObject m_flowerPrefab;
    [SerializeField] private int m_flowerPoolSize = 5;
    [SerializeField] public List<GameObject> m_flowerPool;

    [Header("Egglet Pool")]
    [SerializeField] public Transform m_eggletParent;
    [SerializeField] private GameObject m_eggletPrefab;
    [SerializeField] private int m_eggletPoolSize = 5;
    [SerializeField] public List<GameObject> m_eggletPool;

    [Header("Chick Pool")]
    [SerializeField] public Transform m_chickParent;
    [SerializeField] private GameObject m_chickPrefab;
    [SerializeField] private int m_chickPoolSize = 5;
    [SerializeField] public List<GameObject> m_chickPool;

    [Header("Fledgling Pool")]
    [SerializeField] public Transform m_fledglingParent;
    [SerializeField] private GameObject m_fledglingPrefab;
    [SerializeField] private int m_fledglingPoolSize = 5;
    [SerializeField] public List<GameObject> m_fledglingPool;

    [Header("Bird Pool")]
    [SerializeField] public Transform m_birdParent;
    [SerializeField] private GameObject m_birdPrefab;
    [SerializeField] private int m_birdPoolSize = 5;
    [SerializeField] public List<GameObject> m_birdPool;

    [Header("Spider Pool")]
    [SerializeField] public Transform m_spiderParent;
    [SerializeField] private GameObject m_spiderPrefab;
    [SerializeField] private int m_spiderPoolSize = 5;
    [SerializeField] public List<GameObject> m_spiderPool;

    [Header("Spider Dark Pool")]
    [SerializeField] public Transform m_spiderDarkParent;
    [SerializeField] private GameObject m_spiderDarkPrefab;
    [SerializeField] private int m_spiderDarkPoolSize = 5;
    [SerializeField] public List<GameObject> m_spiderDarkPool;

    [Header("Spider Toxin Pool")]
    [SerializeField] public Transform m_spiderToxinParent;
    [SerializeField] private GameObject m_spiderToxinPrefab;
    [SerializeField] private int m_spiderToxinPoolSize = 5;
    [SerializeField] public List<GameObject> m_spiderToxinPool;

    [Header("Bee Pool")]
    [SerializeField] public Transform m_beeParent;
    [SerializeField] private GameObject m_beePrefab;
    [SerializeField] private int m_beePoolSize = 5;
    [SerializeField] public List<GameObject> m_beePool;

    [Header("Bumble Pale Pool")]
    [SerializeField] public Transform m_bumblePaleParent;
    [SerializeField] private GameObject m_bumblePalePrefab;
    [SerializeField] private int m_bumblePalePoolSize = 5;
    [SerializeField] public List<GameObject> m_bumblePalePool;

    [Header("Bumble Yello Pool")]
    [SerializeField] public Transform m_bumbleYelloParent;
    [SerializeField] private GameObject m_bumbleYelloPrefab;
    [SerializeField] private int m_bumbleYelloPoolSize = 5;
    [SerializeField] public List<GameObject> m_bumbleYelloPool;

    [Header("Sting Pool")]
    [SerializeField] public Transform m_stingParent;
    [SerializeField] private GameObject m_stingPrefab;
    [SerializeField] private int m_stingPoolSize = 5;
    [SerializeField] public List<GameObject> m_stingPool;

    [Header("Bat Pool")]
    [SerializeField] public Transform m_batParent;
    [SerializeField] private GameObject m_batPrefab;
    [SerializeField] private int m_batPoolSize = 5;
    [SerializeField] public List<GameObject> m_batPool;

    [Header("Vampire Pool")]
    [SerializeField] public Transform m_vampireParent;
    [SerializeField] private GameObject m_vampirePrefab;
    [SerializeField] private int m_vampirePoolSize = 5;
    [SerializeField] public List<GameObject> m_vampirePool;

    [Header("BatLord Pool")]
    [SerializeField] public Transform m_batLordParent;
    [SerializeField] private GameObject m_batLordPrefab;
    [SerializeField] private int m_batLordPoolSize = 5;
    [SerializeField] public List<GameObject> m_batLordPool;

    [Header("Spooky Pool")]
    [SerializeField] public Transform m_spookyParent;
    [SerializeField] private GameObject m_spookyPrefab;
    [SerializeField] private int m_spookyPoolSize = 5;
    [SerializeField] public List<GameObject> m_spookyPool;

    [Header("Ghost Pool")]
    [SerializeField] public Transform m_ghostParent;
    [SerializeField] private GameObject m_ghostPrefab;
    [SerializeField] private int m_ghostPoolSize = 5;
    [SerializeField] public List<GameObject> m_ghostPool;

    [Header("Phantom Pool")]
    [SerializeField] public Transform m_phantomParent;
    [SerializeField] private GameObject m_phantomPrefab;
    [SerializeField] private int m_phantomPoolSize = 5;
    [SerializeField] public List<GameObject> m_phantomPool;

    private void Start()
    {
        _instance = this;
        m_seedPool = new List<GameObject>();
        for (int i = 0; i < m_seedPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_seedPrefab, m_seedParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_sproutPool = new List<GameObject>();
        for (int i = 0; i < m_sproutPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_sproutPrefab, m_sproutParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_flowerPool = new List<GameObject>();
        for (int i = 0; i < m_flowerPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_flowerPrefab, m_flowerParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_eggletPool = new List<GameObject>();
        for (int i = 0; i < m_eggletPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_eggletPrefab, m_eggletParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_chickPool = new List<GameObject>();
        for (int i = 0; i < m_chickPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_chickPrefab, m_chickParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_fledglingPool = new List<GameObject>();
        for (int i = 0; i < m_fledglingPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_fledglingPrefab, m_fledglingParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_birdPool = new List<GameObject>();
        for (int i = 0; i < m_birdPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_birdPrefab, m_birdParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_spiderPool = new List<GameObject>();
        for (int i = 0; i < m_spiderPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_spiderPrefab, m_spiderParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_spiderDarkPool = new List<GameObject>();
        for (int i = 0; i < m_spiderDarkPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_spiderDarkPrefab, m_spiderDarkParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_spiderToxinPool = new List<GameObject>();
        for (int i = 0; i < m_spiderToxinPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_spiderToxinPrefab, m_spiderToxinParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_beePool = new List<GameObject>();
        for (int i = 0; i < m_beePoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_beePrefab, m_beeParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_bumblePalePool = new List<GameObject>();
        for (int i = 0; i < m_bumblePalePoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_bumblePalePrefab, m_bumblePaleParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_bumbleYelloPool = new List<GameObject>();
        for (int i = 0; i < m_bumbleYelloPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_bumbleYelloPrefab, m_bumbleYelloParent);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_stingPool = new List<GameObject>();
        for (int i = 0; i < m_stingPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_stingPrefab, m_stingParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_batPool = new List<GameObject>();
        for (int i = 0; i < m_batPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_batPrefab, m_batParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_vampirePool = new List<GameObject>();
        for (int i = 0; i < m_vampirePoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_vampirePrefab, m_vampireParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_batLordPool = new List<GameObject>();
        for (int i = 0; i < m_batLordPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_batLordPrefab, m_batLordParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_spookyPool = new List<GameObject>();
        for (int i = 0; i < m_spookyPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_spookyPrefab, m_spookyParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_ghostPool = new List<GameObject>();
        for (int i = 0; i < m_ghostPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_ghostPrefab, m_ghostParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }

        m_phantomPool = new List<GameObject>();
        for (int i = 0; i < m_phantomPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_phantomPrefab, m_phantomParent);
            MobManager.instance.m_aliveMobs.Add(go);
            BackMob(go.GetComponent<MobAI>());
            go.SetActive(false);
        }
    }

    public GameObject GetFromPool(ref List<GameObject> pool, Transform parent, GameObject prefab)
    {
        lock (pool)
        {
            int lastIndex = pool.Count - 1;
            if (lastIndex >= 0)
            {
                GameObject go = pool[lastIndex];
                Debug.Log($"Get From Pool {go.name}");
                pool.RemoveAt(lastIndex);
                if (go.transform.parent != parent)
                {
                    go.transform.SetParent(parent);
                }
                return go;
            }
            else
            {
                Debug.Log("Get From Pool Instantiate");
                GameObject go = Instantiate<GameObject>(prefab, parent);
                MobManager.instance.m_aliveMobs.Add(go);
                return go;
            }
        }
    }

    public void BackToPool(ref List<GameObject> pool, GameObject go)
    {
        pool.Add(go);
        go.SetActive(false);
    }

    public GameObject GetMob(MobType type)
    {
        switch(type)
        {
            case MobType.seed:
                return GetFromPool(ref m_seedPool, m_seedParent, m_seedPrefab);
            case MobType.sprout:
                return GetFromPool(ref m_sproutPool, m_sproutParent, m_sproutPrefab);
            case MobType.flower:
                return GetFromPool(ref m_flowerPool, m_flowerParent, m_flowerPrefab);
            case MobType.egglet:
                return GetFromPool(ref m_eggletPool, m_eggletParent, m_eggletPrefab);
            case MobType.chick:
                return GetFromPool(ref m_chickPool, m_chickParent, m_chickPrefab);
            case MobType.fledgling:
                return GetFromPool(ref m_fledglingPool, m_fledglingParent, m_fledglingPrefab);
            case MobType.bird:
                return GetFromPool(ref m_birdPool, m_birdParent, m_birdPrefab);
            case MobType.spider:
                return GetFromPool(ref m_spiderPool, m_spiderParent, m_spiderPrefab);
            case MobType.spiderDark:
                return GetFromPool(ref m_spiderDarkPool, m_spiderDarkParent, m_spiderDarkPrefab);
            case MobType.spiderToxin:
                return GetFromPool(ref m_spiderToxinPool, m_spiderToxinParent, m_spiderToxinPrefab);
            case MobType.bee:
                return GetFromPool(ref m_beePool, m_beeParent, m_beePrefab);
            case MobType.bumblePale:
                return GetFromPool(ref m_bumblePalePool, m_bumblePaleParent, m_bumblePalePrefab);
            case MobType.bumbleYello:
                return GetFromPool(ref m_bumbleYelloPool, m_bumbleYelloParent, m_bumbleYelloPrefab);
            case MobType.sting:
                return GetFromPool(ref m_stingPool, m_stingParent, m_stingPrefab);
            case MobType.bat:
                return GetFromPool(ref m_batPool, m_batParent, m_batPrefab);
            case MobType.vampire:
                return GetFromPool(ref m_vampirePool, m_vampireParent, m_vampirePrefab);
            case MobType.batLord:
                return GetFromPool(ref m_batLordPool, m_batLordParent, m_batLordPrefab);
            case MobType.spooky:
                return GetFromPool(ref m_spookyPool, m_spookyParent, m_spookyPrefab);
            case MobType.ghost:
                return GetFromPool(ref m_ghostPool, m_ghostParent, m_ghostPrefab);
            case MobType.phantom:
                return GetFromPool(ref m_phantomPool, m_phantomParent, m_phantomPrefab);
        }
        Debug.Log("Get Mob Fail");
        return null;
    }

    public void BackMob(MobAI mob)
    {
        mob.currentDistrict = -1;
        switch (mob.mobtype)
        {
            case MobType.seed:
                BackToPool(ref m_seedPool, mob.gameObject);
                break;
            case MobType.sprout:
                BackToPool(ref m_sproutPool, mob.gameObject);
                break;
            case MobType.flower:
                BackToPool(ref m_flowerPool, mob.gameObject);
                break;
            case MobType.egglet:
                BackToPool(ref m_eggletPool, mob.gameObject);
                break;
            case MobType.chick:
                BackToPool(ref m_chickPool, mob.gameObject);
                break;
            case MobType.fledgling:
                BackToPool(ref m_fledglingPool, mob.gameObject);
                break;
            case MobType.bird:
                BackToPool(ref m_birdPool, mob.gameObject);
                break;
            case MobType.spider:
                BackToPool(ref m_spiderPool, mob.gameObject);
                break;
            case MobType.spiderDark:
                BackToPool(ref m_spiderDarkPool, mob.gameObject);
                break;
            case MobType.spiderToxin:
                BackToPool(ref m_spiderToxinPool, mob.gameObject);
                break;
            case MobType.bee:
                BackToPool(ref m_beePool, mob.gameObject);
                break;
            case MobType.bumblePale:
                BackToPool(ref m_bumblePalePool, mob.gameObject);
                break;
            case MobType.bumbleYello:
                BackToPool(ref m_bumbleYelloPool, mob.gameObject);
                break;
            case MobType.sting:
                BackToPool(ref m_stingPool, mob.gameObject);
                break;
            case MobType.bat:
                BackToPool(ref m_batPool, mob.gameObject);
                break;
            case MobType.vampire:
                BackToPool(ref m_vampirePool, mob.gameObject);
                break;
            case MobType.batLord:
                BackToPool(ref m_batLordPool, mob.gameObject);
                break;
            case MobType.spooky:
                BackToPool(ref m_spookyPool, mob.gameObject);
                break;
            case MobType.ghost:
                BackToPool(ref m_ghostPool, mob.gameObject);
                break;
            case MobType.phantom:
                BackToPool(ref m_phantomPool, mob.gameObject);
                break;
        }
    }
}
