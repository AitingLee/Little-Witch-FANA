using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MobAI : MonoBehaviour
{
    public MobType mobtype;
    public int currentDistrict;

    [Header("Display")]
    public Animator animator;
    public Slider HPBar;
    public TextMeshProUGUI HPText;
    public GameObject currentMark;
    public GameObject wholeCanvas;

    [Header("State Flag")]
    public FSMSystem m_FSMSystem;
    public bool m_bBattleBefore;
    public bool m_bHandleAttack;
    public bool m_bPerformAttack;
    public bool m_isAttacking;
    public bool m_isHitBack;
    public bool m_isDead;
    public bool m_isInteracting;
    public bool m_isFrozen;
    public bool m_isAmbush;

    [Header("State Value")]
    public float m_fSightDistance;
    public float m_fBattleRange;
    public Vector3 m_vHitBackPos;

    [Header("Combat Value")]
    public PlayerManager playerM;
    public float m_fHp = 100;
    public float m_fMaxHP = 100;
    public float m_fAttackPower = 15;
    public float m_fCriticalRate = 0.3f;
    public float m_skillProbability = 0.2f;

    [Header("Sterring")]
    //All
    public GameObject m_Go;
    public Vector3 m_vTargetPosition;
    public Vector3 m_vCurrentVector;
    public float m_fTempTurnForce;
    public float m_fMoveForce;
    public bool m_bMove;
    public bool m_bCol;
    //Move
    public float m_fGroundOffset;
    public Vector3 m_homePosition;
    public float m_Speed;
    public float m_fMaxSpeed = 0.35f;
    public float m_fRot;
    public float m_fMaxRot = 0.2f;
    //Avoid
    public float m_fRadius = 0.5f;
    public float m_fProbeLength = 8f;
    //Target
    public List<Obstacle> m_AvoidTargets;
    public List<PathPoint> m_thisIslandPoints;

    [Header("Effect")]
    public float m_fFrozenHeightOffset = 0.5f;
    public float m_fFrozenSpan = 6f;
    float m_fFrozenTimer;
    GameObject m_goCurrentEffect;

    [Header("Dissolve")]
    public MaterialPropertyBlock block;
    bool dissolving;
    bool appearing;
    float dissolveValue;
    public Renderer thisRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerM = PlayerManager.instance;
        m_Go = this.gameObject;
        m_homePosition = transform.position;

        if (currentDistrict != -1)
        {
            MobManager.instance.AddDistrictMob(currentDistrict, m_Go);
            m_thisIslandPoints = MobManager.instance.GetDistrictPoints(currentDistrict);
        }
        if (wholeCanvas.activeSelf)
        {
            wholeCanvas.SetActive(false);
        }

        block = new MaterialPropertyBlock();
        HPText.text = $"{m_fHp}";
        HPBar.value = (float)(m_fHp / m_fMaxHP);

    }

    private void Update()
    {
        if (CanvasManager.instance.freezeTime)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }

        if (currentMark != null)
        {
            currentMark.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 4, 0));
        }
        if (m_isFrozen)
        {
            UpdateFrozen();
        }
        if (dissolving)
        {
            Dissolve();
        }
        if (appearing)
        {
            UpdateAppear();
        }
    }

    void FixedUpdate()
    {
        if (m_isHitBack)
        {
            UpdateHitBack();
        }
    }

    private void LateUpdate()
    {
        m_isDead = animator.GetBool("isDead");
        m_isAttacking = animator.GetBool("isAttacking");
        StepGround();
    }

    public void StepGround()
    {
        if (m_isAmbush)
        {
            return;
        }
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 150, Vector3.down);
        Vector3 pos = transform.position;
        if (Physics.Raycast(ray, out hit, 180f, MobManager.instance.m_groundLayer))
        {
            pos.y = hit.point.y + m_fGroundOffset;
            Debug.Log($"mob {gameObject.name} in district {transform.parent.name} step on {hit.transform.gameObject.name} height {hit.point.y}");
        }
        transform.position = pos;
    }

    public void PlayAnimation(string targetAnimation)
    {
        animator.CrossFade(targetAnimation, 0.2f);
    }
    public bool CheckPlayerInSight()
    {
        float playerDistance = (playerM.transform.position - transform.position).magnitude;

        if (playerM.playerData.HP > 0 && playerDistance < m_fSightDistance)
        {
            return true;
        }
        return false;
    }

    public bool CheckPlayerInBattleRange()
    {
        float playerDistance = (playerM.transform.position - transform.position).magnitude;
        if (playerM.playerData.HP > 0 && playerDistance < m_fBattleRange)
        {
            return true;
        }
        return false;
    }

    public bool CheckPlayerInAttackRange(float m_fAttackRange)
    {
        float playerDistance = (playerM.transform.position - transform.position).magnitude;
        if (playerM.playerData.HP > 0 && playerDistance < m_fAttackRange)
        {
            return true;
        }
        return false;
    }

    public void BeAlert()
    {
        DisplayQuestionMark();
        Vector3 lookPos = PlayerManager.instance.transform.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos, Vector3.up);
    }

    public void AlertToAttack()
    {
        if (!m_bBattleBefore)
        {
            m_bBattleBefore = true;
        }
        DisplayExclamationMark();
    }


    public void AnimatorSetBool(string name, bool state)
    {
        animator.SetBool(name, state);
    }
    public void AnimatorSetFloat(string name, float amount)
    {
        animator.SetFloat(name, amount);
    }
    public void AnimatorSetInt(string name, int amount)
    {
        animator.SetInteger(name, amount);
    }

    public void Spawn(int districtNo)
    {
        currentDistrict = districtNo;
        //設定區域資訊
        MobManager.instance.AddDistrictMob(currentDistrict, gameObject);
        m_thisIslandPoints = MobManager.instance.GetDistrictPoints(currentDistrict);

        //設定出生位置
        int spawnPoint = GetSpawnPoint();
        Vector3 cPos = m_thisIslandPoints[spawnPoint].transform.position;
        RaycastHit hit;
        Ray ray = new Ray(cPos + Vector3.up * 5, Vector3.down);
        if (Physics.Raycast(ray, out hit, 50f, MobManager.instance.m_groundLayer))
        {
            cPos.y = hit.point.y + m_fGroundOffset;
        }
        Revive(cPos);

    }

    public void Revive(Vector3 pos)
    {
        transform.position = pos;


        //設定初始資訊
        m_fHp = m_fMaxHP;
        m_isDead = false;
        if (gameObject.tag == "Mob")
        {
            m_bBattleBefore = false;
        }
        else if (gameObject.tag == "Elite")
        {
            m_bBattleBefore = true;
        }

        //開啟遊戲物件
        gameObject.SetActive(true);

        //漸進淡出
        appearing = true;

        //設定顯示UI
        HPText.text = $"{m_fHp}";
        HPBar.value = (float)(m_fHp / m_fMaxHP);

        //初始化參數
        animator = GetComponent<Animator>();
        playerM = PlayerManager.instance;
        m_Go = this.gameObject;
        m_homePosition = transform.position;
        block = new MaterialPropertyBlock();

        //設定動畫
        AnimatorSetBool("isAttacking", false);
        AnimatorSetBool("isDead", false);
        AnimatorSetFloat("moveAmount", 0);

        //設定FSM
        m_FSMSystem.PerformTransition(eFSMTransition.GO_Idle);

        //關閉血條
        if (wholeCanvas.activeSelf)
        {
            wholeCanvas.SetActive(false);
        }
    }

    private void UpdateAppear()
    {
        if (dissolveValue < 0.5f)
        {
            dissolveValue += 0.02f;
            thisRenderer.GetPropertyBlock(block);
            block.SetFloat("_Clipping_Level", dissolveValue);
            thisRenderer.SetPropertyBlock(block);

            if (mobtype == MobType.egglet)
            {
                dissolveValue = 0.5f;
            }
        }
        else
        {
            dissolveValue = 0.5f;
            thisRenderer.GetPropertyBlock(block);
            block.SetFloat("_Clipping_Level", dissolveValue);
            thisRenderer.SetPropertyBlock(block);
            appearing = false;
        }
    }

    public void PopUp()
    {
        PlayAnimation("Spawn");
    }

    public void FinishPopUp()
    {
        m_homePosition = transform.position;
        m_isAmbush = false;
        m_bBattleBefore = true;
        if (!wholeCanvas.activeSelf)
        {
            wholeCanvas.SetActive(true);
        }
    }

    private int GetSpawnPoint()
    {
        int spawnPoint = UnityEngine.Random.Range(0, m_thisIslandPoints.Count);
        Vector3 cPos = m_thisIslandPoints[spawnPoint].transform.position;
        for (int i = 0; i < MobManager.instance.districtMobs[currentDistrict].Count; i++)
        {
            if (Vector3.Distance(MobManager.instance.districtMobs[currentDistrict][i].transform.position, cPos) < 5f)
            {
                Debug.Log($"current dist mob {i} too close to spawnpoint {spawnPoint}");
                spawnPoint = GetSpawnPoint();
            }
            else
            {
                Debug.Log($"current dist mob {i} not close to spawnpoint {spawnPoint}");
            }
        }
        Debug.Log($"return spawn point {spawnPoint}");
        return spawnPoint;
    }

    public void TakeDamage(int attack)
    {

        if (!m_isDead)
        {
            PlayAnimation("TakeDamage");
            m_fHp -= attack;
            if (m_fHp <= 0)
            {
                m_fHp = 0;
            }
            HPText.text = $"{m_fHp}";
            HPBar.value = m_fHp / m_fMaxHP;
            if (!m_bBattleBefore)
            {
                DisplayExclamationMark();
                m_bBattleBefore = true;
            }
        }
        if (!wholeCanvas.activeSelf)
        {
            wholeCanvas.SetActive(true);
        }
        if (m_isAttacking)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    public void HandleHitBack()
    {
        if (mobtype == MobType.dragon) { return; }
        m_isInteracting = true;
        m_isHitBack = true;
        m_vHitBackPos = transform.position + playerM.transform.forward * 3;
        DisplayAngryMark();
    }

    public void UpdateHitBack()
    {
        Vector3 planePos = transform.position;
        planePos.y = 0;
        Vector3 planeBackPos = m_vHitBackPos;
        planeBackPos.y = 0;
        Debug.Log($"Distance = {Vector3.Distance(planePos, planeBackPos)}");
        if (m_isHitBack && Vector3.Distance(planePos, planeBackPos) > 1)
        {
            m_bMove = false;
            Vector3 moveToPos = Vector3.Lerp(transform.position, m_vHitBackPos, 0.2f);
            RaycastHit hit;
            Ray ray = new Ray(moveToPos + Vector3.up * 3, Vector3.down);
            if (Physics.Raycast(ray, out hit, 5f, MobManager.instance.m_groundLayer))
            {
                moveToPos.y = hit.point.y + m_fGroundOffset;
            }
            transform.position = moveToPos;
        }
        else
        {
            m_FSMSystem.PerformTransition(eFSMTransition.GO_Idle);
            m_bMove = true;
            m_isInteracting = false;
            m_isHitBack = false;
        }
    }


    public void DisplayQuestionMark()
    {
        DisableCurrentMark();
        currentMark = ObjectPoolManager.instance.GetQuestionMark();
        currentMark.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 4, 0));
        currentMark.SetActive(true);
    }

    public void DisableCurrentMark()
    {
        if (currentMark != null)
        {
            currentMark.SetActive(false);
        }
    }

    public void DisplayExclamationMark()
    {
        DisableCurrentMark();
        currentMark = ObjectPoolManager.instance.GetExclamationMark();
        currentMark.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 4, 0));
        currentMark.SetActive(true);
    }

    public void DisplayAngryMark()
    {
        DisableCurrentMark();
        currentMark = ObjectPoolManager.instance.GetAngryMark();
        currentMark.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 4, 0));
        currentMark.SetActive(true);
    }

    public void Disappear()
    {
        //Animator呼叫Disappear > 開始update Dissolve > 溶解成透明後呼叫 Disable
        Debug.Log($"{gameObject.name}Dissappear");
        dissolving = true;
        dissolveValue = 0.5f;
        TaskManager.instance.AddKilledMob(this);
        DropItem();
        wholeCanvas.SetActive(false);
    }

    public void Dissolve()
    {
        Debug.Log($"dissolveValue = {dissolveValue}");
        if (dissolveValue > 0.05f)
        {
            dissolveValue -= 0.05f;
            thisRenderer.GetPropertyBlock(block);
            block.SetFloat("_Clipping_Level", dissolveValue);
            thisRenderer.SetPropertyBlock(block);
        }
        else
        {
            block.SetFloat("_Clipping_Level", 0);
            dissolving = false;
            Disable();
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        MobManager.instance.RemoveDistrictMob(currentDistrict, m_Go);
        MobPoolManager.instance.BackMob(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>())
        {
            Debug.Log("Stop");
            m_vTargetPosition = transform.position;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tornado" && gameObject.tag != "Elite")
        {
            m_isInteracting = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Tornado" && gameObject.tag != "Elite")
        {
            transform.RotateAround(other.transform.position, Vector3.up, 5f);
        }
    }

    public void ExitTornado()
    {
        Vector3 landPos = transform.position;
        RaycastHit hit;
        Ray ray = new Ray(landPos, Vector3.down);
        if (Physics.Raycast(ray, out hit, 10f, MobManager.instance.m_groundLayer))
        {
            landPos.y = hit.point.y + m_fGroundOffset;
        }
        transform.position = landPos;
        if (!m_isDead)
        {
            transform.LookAt(playerM.transform.position, Vector3.up);
        }
        m_isInteracting = false;
    }
    
    public void BeFrozen()
    {
        m_goCurrentEffect = ObjectPoolManager.instance.GetFrozenEffect();
        m_goCurrentEffect.transform.SetParent(transform);
        m_goCurrentEffect.transform.localPosition = new Vector3(0, m_fFrozenHeightOffset, 0);
        m_goCurrentEffect.transform.rotation = Quaternion.identity;
        m_isFrozen = true;
        m_goCurrentEffect.SetActive(true);
        m_fFrozenTimer = Time.time;
    }
    
    public void UpdateFrozen()
    {
        if (Time.time - m_fFrozenTimer >= m_fFrozenSpan || m_isDead)
        {
            m_isFrozen = false;
            m_goCurrentEffect.SetActive(false);
        }
    }

    public void DropItem()
    {
        if (mobtype == MobType.dragon)
        {
            return;
        }
        //產生掉落物清單
        List<ItemInformation> dropItems = MobManager.instance.RandonDropList(mobtype);
        bool gold = false;
        GameObject orbGO;
        
        //逐一查詢如果掉落物有裝備或寶石 gold =true
        foreach (ItemInformation item in dropItems)
        {
            if (item.itemSO.itemType == ItemType.equipment || item.itemSO.itemType == ItemType.gem)
            {
                gold = true;
            }
        }

        //如果gold = true 遊戲物件取金色光球，否則為一般光球
        if (gold)
        {
            orbGO = ObjectPoolManager.instance.GetLightSpray();
        }
        else
        {
            orbGO = ObjectPoolManager.instance.GetLightOrb();
        }

        //將產生的掉落物清單塞到光球內
        LightOrb lightOrb = orbGO.GetComponent<LightOrb>();
        Debug.Log($"mob dead create drop item list count = {dropItems.Count}");
        lightOrb.orbItemInfos = dropItems;

        //光球位置為怪物位置加偏移後顯示
        orbGO.transform.position = transform.position + Vector3.up * 0.5f;
        orbGO.SetActive(true);
    }


}
