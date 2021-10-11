using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager _instance;

    public static ObjectPoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ObjectPoolManager();
            }

            return _instance;
        }
    }

    [Header("Left Attack FX Pool")]
    [SerializeField] public Transform m_hitFXParent;
    [SerializeField] private GameObject m_hitFXPrefab;
    [SerializeField] private int m_hitFXPoolSize = 5;
    [SerializeField] public List<GameObject> m_hitFXPool;
    [SerializeField] public Transform m_waveFXParent;
    [SerializeField] private GameObject m_waveFXPrefab;
    [SerializeField] private int m_waveFXPoolSize = 2;
    [SerializeField] public List<GameObject> m_waveFXPool;

    [Header("Right Attack Magic Ball FX Pool")]
    [SerializeField] public Transform m_magicBallParent;
    [SerializeField] private GameObject m_magicBallPrefab;
    [SerializeField] private int m_magicBallPoolSize = 5;
    [SerializeField] public List<GameObject> m_magicBallPool;

    [Header("Skill A Tornado FX Pool")]
    [SerializeField] public Transform m_tornadoParent;
    [SerializeField] private GameObject m_tornadoPrefab;
    [SerializeField] private int m_tornadoPoolSize = 2;
    [SerializeField] public List<GameObject> m_tornadoPool;

    [Header("Skill B Spike FX Pool")]
    [SerializeField] public Transform m_spikeParent;
    [SerializeField] private GameObject m_spikePrefab;
    [SerializeField] private int m_spikePoolSize = 2;
    [SerializeField] public List<GameObject> m_spikePool;
    [SerializeField] public Transform m_spikeUnitParent;
    [SerializeField] private GameObject m_spikeUnitPrefab;
    [SerializeField] private int m_spikeUnitPoolSize = 10;
    [SerializeField] public List<GameObject> m_spikeUnitPool;
    [SerializeField] public Transform m_frozenEffectParent;
    [SerializeField] private GameObject m_frozenEffectPrefab;
    [SerializeField] private int m_frozenEffectPoolSize = 5;
    [SerializeField] public List<GameObject> m_frozenEffectPool;

    [Header("Skill D Fire Rain FX Pool")]
    [SerializeField] public Transform m_fireRainParent;
    [SerializeField] private GameObject m_fireRainPrefab;
    [SerializeField] private int m_fireRainPoolSize = 2;
    [SerializeField] public List<GameObject> m_fireRainPool;

    [Header("Damage Num Pool")]
    [SerializeField] public Transform m_damageNumParent;
    [SerializeField] private GameObject m_damageNumPrefab;
    [SerializeField] private int m_damageNumPoolSize = 5;
    [SerializeField] public List<GameObject> m_damageNumPool;

    [Header("Question Mark Pool")]
    [SerializeField] public Transform m_questionMarkParent;
    [SerializeField] private GameObject m_questionMarkPrefab;
    [SerializeField] private int m_questionMarkPoolSize = 5;
    [SerializeField] public List<GameObject> m_questionMarkPool;

    [Header("Exclamation Mark Pool")]
    [SerializeField] public Transform m_exclamationMarkParent;
    [SerializeField] private GameObject m_exclamationMarkPrefab;
    [SerializeField] private int m_exclamationMarkSize = 5;
    [SerializeField] public List<GameObject> m_exclamationMarkPool;

    [Header("Angry Mark Pool")]
    [SerializeField] public Transform m_angryMarkParent;
    [SerializeField] private GameObject m_angryMarkPrefab;
    [SerializeField] private int m_angryMarkPoolSize = 5;
    [SerializeField] public List<GameObject> m_angryMarkPool;

    [Header("Web Projectile Pool")]
    [SerializeField] public Transform m_webProjectileParent;
    [SerializeField] private GameObject m_webProjectilePrefab;
    [SerializeField] private int m_webProjectilePoolSize = 5;
    [SerializeField] public List<GameObject> m_webProjectilePool;

    [Header("Toxin Poison Pool")]
    [SerializeField] public Transform m_toxinPoisonParent;
    [SerializeField] private GameObject m_toxinPoisonPrefab;
    [SerializeField] private int m_toxinPoisonPoolSize = 5;
    [SerializeField] public List<GameObject> m_toxinPoisonPool;

    [Header("Fire Land Pool")]
    [SerializeField] public Transform m_fireLandParent;
    [SerializeField] private GameObject m_fireLandPrefab;
    [SerializeField] private int m_fireLandPoolSize = 5;
    [SerializeField] public List<GameObject> m_fireLandPool;

    [Header("Press E Pool")]
    [SerializeField] public Transform m_pressEParent;
    [SerializeField] private GameObject m_pressEPrefab;
    [SerializeField] private int m_pressEPoolSize = 2;
    [SerializeField] public List<GameObject> m_pressEPool;

    [Header("Light Orb Pool")]
    [SerializeField] public Transform m_lightOrbParent;
    [SerializeField] private GameObject m_lightOrbPrefab;
    [SerializeField] private int m_lightOrbPoolSize = 10;
    [SerializeField] public List<GameObject> m_lightOrbPool;

    [Header("Light Spray Pool")]
    [SerializeField] public Transform m_lightSprayParent;
    [SerializeField] private GameObject m_lightSprayPrefab;
    [SerializeField] private int m_lightSprayPoolSize = 3;
    [SerializeField] public List<GameObject> m_lightSprayPool;

    [Header("Fire Ball Pool")]
    [SerializeField] public Transform m_fireBallParent;
    [SerializeField] private GameObject m_fireBallPrefab;
    [SerializeField] private int m_lfireBallPoolSize = 10;
    [SerializeField] public List<GameObject> m_fireBallPool;



    private void Start()
    {
        _instance = this;
        m_hitFXPool = new List<GameObject>();
        for (int i = 0; i < m_hitFXPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_hitFXPrefab, m_hitFXParent);
            go.SetActive(false);
        }

        m_waveFXPool = new List<GameObject>();
        for (int i = 0; i < m_waveFXPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_waveFXPrefab, m_waveFXParent);
            go.SetActive(false);
        }

        m_magicBallPool = new List<GameObject>();
        for (int i = 0; i < m_magicBallPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_magicBallPrefab, m_magicBallParent);
            go.SetActive(false);
        }

        m_tornadoPool = new List<GameObject>();
        for (int i = 0; i < m_tornadoPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_tornadoPrefab, m_tornadoParent);
            go.SetActive(false);
        }

        m_spikePool = new List<GameObject>();
        for (int i = 0; i < m_spikePoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_spikePrefab, m_spikeParent);
            go.SetActive(false);
        }

        m_spikeUnitPool = new List<GameObject>();
        for (int i = 0; i < m_spikeUnitPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_spikeUnitPrefab, m_spikeUnitParent);
            go.SetActive(false);
        }

        m_frozenEffectPool = new List<GameObject>();
        for (int i = 0; i < m_frozenEffectPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_frozenEffectPrefab, m_frozenEffectParent);
            go.SetActive(false);
        }

        m_fireRainPool = new List<GameObject>();
        for (int i = 0; i < m_fireRainPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_fireRainPrefab, m_fireRainParent);
            go.SetActive(false);
        }

        m_damageNumPool = new List<GameObject>();
        for (int i = 0; i < m_damageNumPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_damageNumPrefab, m_damageNumParent);
            go.SetActive(false);
        }

        m_questionMarkPool = new List<GameObject>();
        for (int i = 0; i < m_questionMarkPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_questionMarkPrefab, m_questionMarkParent);
            go.SetActive(false);
        }

        m_exclamationMarkPool = new List<GameObject>();
        for (int i = 0; i < m_exclamationMarkSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_exclamationMarkPrefab, m_exclamationMarkParent);
            go.SetActive(false);
        }

        m_angryMarkPool = new List<GameObject>();
        for (int i = 0; i < m_angryMarkPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_angryMarkPrefab, m_angryMarkParent);
            go.SetActive(false);
        }

        m_webProjectilePool = new List<GameObject>();
        for (int i = 0; i < m_webProjectilePoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_webProjectilePrefab, m_webProjectileParent);
            go.SetActive(false);
        }

        m_pressEPool = new List<GameObject>();
        for (int i = 0; i < m_pressEPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_pressEPrefab, m_pressEParent);
            go.SetActive(false);
        }

        m_lightOrbPool = new List<GameObject>();
        for (int i = 0; i < m_lightOrbPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_lightOrbPrefab, m_lightOrbParent);
            go.SetActive(false);
        }

        m_lightSprayPool = new List<GameObject>();
        for (int i = 0; i < m_lightSprayPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_lightSprayPrefab, m_lightSprayParent);
            go.SetActive(false);
        }

        m_fireLandPool = new List<GameObject>();
        for (int i = 0; i < m_fireLandPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_fireLandPrefab, m_fireLandParent);
            go.SetActive(false);
        }

        m_toxinPoisonPool = new List<GameObject>();
        for (int i = 0; i < m_toxinPoisonPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_toxinPoisonPrefab, m_toxinPoisonParent);
            go.SetActive(false);
        }


        m_fireBallPool = new List<GameObject>();
        for (int i = 0; i < m_fireLandPoolSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_fireBallPrefab, m_fireBallParent);
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
                pool.RemoveAt(lastIndex);
                if (go.transform.parent != parent)
                {
                    go.transform.SetParent(parent);
                }
                return go;
            }
            else
            {
                GameObject go = Instantiate<GameObject>(prefab, parent);
                return go;
            }
        }
    }

    public void BackToPool(ref List<GameObject> pool, GameObject go)
    {
        pool.Add(go);
        go.SetActive(false);
    }

    public GameObject GetHitFX()
    {
        return GetFromPool(ref m_hitFXPool, m_hitFXParent, m_hitFXPrefab);
    }

    public void BackHitFX(GameObject go)
    {
        BackToPool(ref m_hitFXPool, go);
    }
    public GameObject GetWave()
    {
        return GetFromPool(ref m_waveFXPool, m_waveFXParent, m_waveFXPrefab);
    }

    public void BackWave(GameObject go)
    {
        BackToPool(ref m_waveFXPool, go);
    }

    public GameObject GetMagicBall()
    {
        return GetFromPool(ref m_magicBallPool, m_magicBallParent, m_magicBallPrefab);
    }

    public void BackMagicBall(GameObject go)
    {
        BackToPool(ref m_magicBallPool, go);
    }

    public GameObject GetTornado()
    {
        return GetFromPool(ref m_tornadoPool, m_tornadoParent, m_tornadoPrefab);
    }

    public void BackTornado(GameObject go)
    {
        BackToPool(ref m_tornadoPool, go);
    }
    public GameObject GetSpike()
    {
        return GetFromPool(ref m_spikePool, m_spikeParent, m_spikePrefab);
    }

    public void BackSpike(GameObject go)
    {
        BackToPool(ref m_spikePool, go);
    }
    public GameObject[] Get10SpikeUnit()
    {
        GameObject[] spikes = new GameObject[10];
        for (int i = 0; i <10; i++)
        {
            spikes[i] = GetFromPool(ref m_spikeUnitPool, m_spikeUnitParent, m_spikeUnitPrefab);
        }
        return spikes;
    }

    public void BackSpikeUnit(GameObject go)
    {
        BackToPool(ref m_spikeUnitPool, go);
    }
    public GameObject GetFrozenEffect()
    {
        return GetFromPool(ref m_frozenEffectPool, m_frozenEffectParent, m_frozenEffectPrefab);
    }

    public void BackFrozenEffect(GameObject go)
    {
        BackToPool(ref m_frozenEffectPool, go);
    }
    public GameObject GetFireRain()
    {
        return GetFromPool(ref m_fireRainPool, m_fireRainParent, m_fireRainPrefab);
    }

    public void BackFireRain(GameObject go)
    {
        BackToPool(ref m_fireRainPool, go);
    }
    public GameObject GetDamageNum()
    {
        return GetFromPool(ref m_damageNumPool, m_damageNumParent, m_damageNumPrefab);
    }

    public void BackDamageNum(GameObject go)
    {
        BackToPool(ref m_damageNumPool, go);
    }

    public GameObject GetQuestionMark()
    {
        return GetFromPool(ref m_questionMarkPool, m_questionMarkParent, m_questionMarkPrefab);
    }

    public void BackQuestionMark(GameObject go)
    {
        BackToPool(ref m_questionMarkPool, go);
    }

    public GameObject GetExclamationMark()
    {
        return GetFromPool(ref m_exclamationMarkPool, m_exclamationMarkParent, m_exclamationMarkPrefab);
    }

    public void BackExclamationMark(GameObject go)
    {
        BackToPool(ref m_exclamationMarkPool, go);
    }

    public GameObject GetAngryMark()
    {
        return GetFromPool(ref m_angryMarkPool, m_angryMarkParent, m_angryMarkPrefab);
    }

    public void BackAngryMark(GameObject go)
    {
        BackToPool(ref m_angryMarkPool, go);
    }

    public GameObject GetWebProjectile()
    {
        return GetFromPool(ref m_webProjectilePool, m_webProjectileParent, m_webProjectilePrefab);
    }

    public void BackWebProjectile(GameObject go)
    {
        BackToPool(ref m_webProjectilePool, go);
    }

    public GameObject GetToxinPoison()
    {
        return GetFromPool(ref m_toxinPoisonPool, m_toxinPoisonParent, m_toxinPoisonPrefab);
    }

    public void BackToxinPoison(GameObject go)
    {
        BackToPool(ref m_toxinPoisonPool, go);
    }

    public GameObject GetPressE()
    {
        return GetFromPool(ref m_pressEPool, m_pressEParent, m_pressEPrefab);
    }

    public void BackPressE(GameObject go)
    {
        BackToPool(ref m_pressEPool, go);
    }

    public GameObject GetLightOrb()
    {
        return GetFromPool(ref m_lightOrbPool, m_lightOrbParent, m_lightOrbPrefab);
    }

    public void BackLightOrb(GameObject go)
    {
        BackToPool(ref m_lightOrbPool, go);
    }

    public GameObject GetLightSpray()
    {
        return GetFromPool(ref m_lightSprayPool, m_lightSprayParent, m_lightSprayPrefab);
    }

    public void BackLightSpray(GameObject go)
    {
        BackToPool(ref m_lightSprayPool, go);
    }

    public GameObject GetFireLand()
    {
        return GetFromPool(ref m_fireLandPool, m_fireLandParent, m_fireLandPrefab);
    }

    public void BackFireLand(GameObject go)
    {
        BackToPool(ref m_fireLandPool, go);
    }
    public GameObject GetFireBall()
    {
        return GetFromPool(ref m_fireBallPool, m_fireBallParent, m_fireBallPrefab);
    }

    public void BackFireBall(GameObject go)
    {
        BackToPool(ref m_fireBallPool, go);
    }

}
