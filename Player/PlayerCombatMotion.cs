using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCombatMotion : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    MobManager mobManager;
    List<GameObject> m_AllMobs;
    CanvasManager canvasManager;
    CameraManager cameraManager;

    [Header("Power")]
    public float LAttackPower = 1;
    public float LHeavyAttackPower = 1.2f;
    public float RAttackPower = 1.4f;
    public float TornadoPower = 0.4f;
    public float IceSpikePower = 0.8f;
    public float FireRainPower = 0.6f;

    [Header("Mana")]
    public int LAttackAddMana = 5;
    public int ShieldConsumeMana = 20;
    public int FireRainConsumeMana = 40;
    public int IceSpikeConsumeMana = 60;
    public int TornadoConsumeMana = 50;

    [Header("Learned Spell")]
    public bool shieldLearned;
    public bool fireRainLearned;
    public bool iceSpikeLearned;
    public bool tornadoLearned;

    [Header("Cool Down")]
    public float shieldCDSpan = 15;
    public float fireRainCDSpan = 30;
    public float iceSpikeCDSpan = 20;
    public float tornadoCDSpan = 25;
    public float tornadoCDTime;
    public float shieldCDTime;
    public float fireRainCDTime;
    public float iceSpikeCDTime;


    bool comboPossible;

    [Header("Dodge")]
    public bool damageAvoid;

    [Header("FX")]
    public Vector3 fireLookAt;

    [Header("Aim")]
    public bool isAiming;
    public Vector3 targetPoint;

    bool waitAnimFinish;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void Start()
    {
        mobManager = MobManager.instance;
        canvasManager = CanvasManager.instance;
        cameraManager = CameraManager.instance;
    }

    private void Update()
    {
        if (CanvasManager.instance.freezeTime)
        {
            return;
        }
        UpdateAllCD();
    }

    public void HandleSkill(int No)
    {
        if (playerManager.isInteracting)
            return;
        if (playerLocomotion.isFlying)
            return;
        if (waitAnimFinish)
            return;
        if (isAiming)
            return;

        switch (No)
        {
            case 0:
                if (shieldCDTime <= 0)
                {
                    HandleSkillShield();
                }
                break;
            case 1:
                if (fireRainCDTime <= 0)
                {
                    HandleSkillFireRain();
                }
                break;
            case 2:
                if (tornadoCDTime <= 0)
                {
                    HandleSkillTornado();
                }
                break;
            case 3:
                if (iceSpikeCDTime <= 0)
                {
                    HandleSkillSpike();
                }
                break;
            default:
                animatorManager.animator.SetBool("isAttacking", false);
                break;
        }

    }

    public void HandleLeftAttack()
    {
        if (playerLocomotion.isFlying)
            return;
        if (waitAnimFinish)
            return;
        if (isAiming)
            return;

        animatorManager.animator.SetBool("isAttacking", true);

            int cururetComboStep = animatorManager.animator.GetInteger("ComboStep");
            if (cururetComboStep == 0)
            {
                animatorManager.PlayTargetAnimation("LAttack", true, true);
                animatorManager.animator.SetInteger("ComboStep", 1) ;
                cururetComboStep = 1;
            }
            if (cururetComboStep != 0)
            {
                if (comboPossible)
                {
                    comboPossible = false;
                    animatorManager.animator.SetInteger("ComboStep", cururetComboStep + 1);
                    animatorManager.animator.SetBool("isInteracting", true);
                    animatorManager.animator.SetBool("isUsingRootMotion", true);
                }
            }
    }

    public void HandleAim()
    {
        //按下Q 開啟或關閉
        if (!isAiming)
        {
            isAiming = true;
            Vector3 targetForward = Camera.main.transform.forward;
            targetForward.y = 0;
            transform.forward = targetForward;
            canvasManager.DisplayAimScreen();
            cameraManager.EnterAimMode();
            playerLocomotion.EnterAimMode();
            animatorManager.PlayTargetAnimation("Aim", true, true);
            animatorManager.animator.SetBool("isAim", true);
            animatorManager.animator.SetBool("isInteracting", true);
            animatorManager.animator.SetBool("isUsingRootMotion", true);
        }
        else
        {
            LookTargetPoint();
            FinishAim();
        }

    }

    public void LookTargetPoint()
    {
        Vector3 lookPos = targetPoint;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
    }

    public void FinishAim()
    {
        isAiming = false;
        canvasManager.HideAimScreen();
        animatorManager.animator.SetBool("isAim", false);
        animatorManager.animator.SetBool("isInteracting", false);
        animatorManager.animator.SetBool("isUsingRootMotion", false);
        cameraManager.ExitAimMode();
        playerLocomotion.ExitAimMode();
        waitAnimFinish = false;
        animatorManager.animator.SetBool("Fire", false);
    }

    public void HandleRightAttack()
    {
        if (waitAnimFinish)
        {
            Debug.Log("waitAnimFinish");
            return;
        }
        animatorManager.animator.SetBool("Fire", true);
        animatorManager.animator.SetBool("isInteracting", true);
        animatorManager.animator.SetBool("isUsingRootMotion", true);
        //發射子彈
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))//如果射線碰撞到物體
        {
            Debug.Log($"Fire RayCast hit {hitInfo.transform.name}");
            targetPoint = hitInfo.point;//記錄碰撞的目標點
        }
        else//射線沒有碰撞到目標點
        {
            //將目標點設定在主角面向
            Debug.Log("Fire RayCast None");
            targetPoint = transform.forward * 5f;
        }
    }

    public void FinishFire()
    {
        animatorManager.animator.SetBool("Fire", false);
    }
    public void StartDetectCombo()
    {
        comboPossible = true;
    }
    public void ResetCombo()
    {
        comboPossible = false;
        animatorManager.animator.SetInteger("ComboStep", 0);
        animatorManager.animator.SetBool("isInteracting", false);
        animatorManager.animator.SetBool("isUsingRootMotion", false);
        animatorManager.animator.SetBool("isAttacking", false);
    }
    public void LeftAttackDetect()
    {
        m_AllMobs = mobManager.GetMobs();
        Vector3 playerPlanePos = transform.position;
        playerPlanePos.y = 0;
        List<int> targets = new List<int>();
        int count = 0;
        foreach (GameObject mob in m_AllMobs)
        {
            if (mob.activeSelf && !mob.GetComponent<MobAI>().m_isDead)
            {
                Vector3 mobPlanePos = mob.transform.position;
                mobPlanePos.y = 0;
                Vector3 mobDir = mobPlanePos - playerPlanePos;
                float distance = mobDir.magnitude;
                float tmpAngle = Vector3.Angle(mobDir.normalized, transform.forward);
                if (distance <= 5f)
                {
                    if (tmpAngle <= 60)
                    {
                        CalculateDamage(LAttackPower, mob.GetComponent<MobAI>(),true);
                        PlayerManager.instance.AddManaValue(LAttackAddMana);
                        targets.Add(count);
                    }
                }
            }
            count++;
        }
        if (targets.Count != 0)
        {
            Vector3 lookPlanePos = m_AllMobs[targets[0]].transform.position;
            lookPlanePos.y = transform.position.y;
            transform.LookAt(lookPlanePos, Vector3.up);
        }
    }

    public void LeftHeavyAttackDetect()
    {
        m_AllMobs = mobManager.GetMobs();
        Vector3 playerPlanePos = transform.position;
        playerPlanePos.y = 0;
        List<int> targets = new List<int>();
        int count = 0;
        foreach (GameObject mob in m_AllMobs)
        {
            if (mob.activeSelf && !mob.GetComponent<MobAI>().m_isDead)
            {
                Vector3 mobPlanePos = mob.transform.position;
                mobPlanePos.y = 0;
                Vector3 mobDir = mobPlanePos - playerPlanePos;
                float distance = mobDir.magnitude;
                float tmpAngle = Vector3.Angle(mobDir.normalized, transform.forward);
                if (distance <= 8f)
                {
                    if (tmpAngle <= 80)
                    {
                        CalculateDamage(LHeavyAttackPower, mob.GetComponent<MobAI>(), true);
                        PlayerManager.instance.AddManaValue(LAttackAddMana);
                        mob.GetComponent<MobAI>().HandleHitBack();
                        targets.Add(count);
                    }
                }
            }
            count++;
        }
        if (targets.Count != 0)
        {
            Vector3 lookPos = m_AllMobs[targets[0]].transform.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos, Vector3.up);
        }
    }

    public void HandleSkillTornado()      //龍捲風
    {
        if (!tornadoLearned)
        {
            return;
        }
        if (PlayerManager.instance.playerData.MP > TornadoConsumeMana)
        {
            PlayerManager.instance.AddManaValue(-TornadoConsumeMana);
            animatorManager.PlayTargetAnimation("SkillTornado", true, true);
            StartTornadoCD();
            animatorManager.animator.SetBool("isAttacking", true);
        }
    }
    public void HandleSkillSpike()      //冰錐
    {
        if (!iceSpikeLearned)
        {
            return;
        }
        if (PlayerManager.instance.playerData.MP > IceSpikeConsumeMana)
        {
            PlayerManager.instance.AddManaValue(-IceSpikeConsumeMana);
            animatorManager.PlayTargetAnimation("SkillSpike", true, true);
            StartIceSpikeCD();
            animatorManager.animator.SetBool("isAttacking", true);
        }
    }
    public void HandleSkillShield()      //土石
    {
        if (!shieldLearned)
        {
            return;
        }
        if (PlayerManager.instance.playerData.MP > ShieldConsumeMana)
        {
            PlayerManager.instance.AddManaValue(-ShieldConsumeMana);
            animatorManager.PlayTargetAnimation("SkillShield", true, true);
            StartDodge();
            Invoke("EndDodge", 8f);
            StartShieldCD();
            animatorManager.animator.SetBool("isAttacking", true);
        }
    }
    public void HandleSkillFireRain()      //火流星
    {
        if (!fireRainLearned)
        {
            return;
        }
        if (PlayerManager.instance.playerData.MP > FireRainConsumeMana)
        {
            PlayerManager.instance.AddManaValue(-FireRainConsumeMana);
            animatorManager.PlayTargetAnimation("SkillFireRain", true, true);
            StartFireRainCD();
            animatorManager.animator.SetBool("isAttacking", true);
        }
    }

    public void CalculateDamage(float skillPower, MobAI target, bool playFX)
    {
        int cri = Random.Range(0, 100);
        float dmgRate = 0;
        if (cri < playerManager.playerData.crit * 100)
        {
            dmgRate = Random.Range(1.6f, 2.0f);
        }
        else
        {
            dmgRate = Random.Range(0.8f, 1.2f);
        }
        int damage = Mathf.RoundToInt(playerManager.playerData.atk * skillPower * dmgRate);
        target.TakeDamage(damage);

        if (playFX)
        {
            Vector3 hitPoint = (target.transform.position + transform.position) * 0.5f;
            GameObject hitFX = ObjectPoolManager.instance.GetHitFX();
            hitFX.transform.position = hitPoint + new Vector3(0, 1, 0);
            hitFX.SetActive(true);
        }
        DisplayDamageNum(damage, dmgRate, target.transform.position + new Vector3(0, 4, 0));
    }

    public void DisplayDamageNum(float damage, float sizeRate, Vector3 position)
    {
        GameObject dmgNum = ObjectPoolManager.instance.GetDamageNum();
        char[] nums = damage.ToString().ToCharArray();
        string dmgText = "";
        foreach (char n in nums)
        {
            dmgText += $"<sprite={n}>";
        }
        dmgNum.GetComponent<TextMeshProUGUI>().text = dmgText;
        dmgNum.GetComponent<TextMeshProUGUI>().fontSize = 8 * sizeRate;
        dmgNum.transform.position = Camera.main.WorldToScreenPoint(position);
        dmgNum.SetActive(true);
    }

    public void HandleDamage()
    {
        FinishAim();
        ResetCombo();
        animatorManager.PlayTargetAnimation("TakeDamage", true, true);
        EndWaitAnim();
    }

    public void HandleDodge()
    {
        if (playerManager.isInteracting)
            return;
        if (playerLocomotion.isFlying)
            return;
        if (PlayerManager.instance.playerData.energy >= 2f)
        {
            animatorManager.PlayTargetAnimation("Dodge", true, true);
            DodgeConsumeEnergy();
        }
    }

    public void StartDodge()
    {
        damageAvoid = true;
    }

    public void EndDodge()
    {
        damageAvoid = false;
    }

    public void DodgeConsumeEnergy()
    {
        PlayerManager.instance.DisplayEnergyBar(true);
        PlayerManager.instance.playerData.energy -= 2f;
    }

    public void StartWaitAnim()
    {
        waitAnimFinish = true;
    }

    public void EndWaitAnim()
    {
        waitAnimFinish = false;
    }


    public void StartShieldCD()
    {
        shieldCDTime = shieldCDSpan;
    }

    public void StartFireRainCD()
    {
        fireRainCDTime = fireRainCDSpan;
    }

    public void StartIceSpikeCD()
    {
        iceSpikeCDTime = iceSpikeCDSpan;
    }

    public void StartTornadoCD()
    {
        tornadoCDTime = tornadoCDSpan;
    }

    public void UpdateAllCD()
    {
        if (shieldCDTime > 0)
        {
            shieldCDTime -= Time.deltaTime;
        }
        if (fireRainCDTime > 0)
        {
            fireRainCDTime -= Time.deltaTime;
        }
        if (iceSpikeCDTime > 0)
        {
            iceSpikeCDTime -= Time.deltaTime;
        }
        if (tornadoCDTime > 0)
        {
            tornadoCDTime -= Time.deltaTime;
        }
    }

    public void LearnSpell(TotemElement element)
    {
        switch (element)
        {
            case TotemElement.earth:
                shieldLearned = true;
                playerManager.shieldIcon.SetActive(true);
                CanvasManager.instance.skillPanel.OnLearnSkill(0);
                break;
            case TotemElement.fire:
                fireRainLearned = true;
                playerManager.fireRainIcon.SetActive(true);
                CanvasManager.instance.skillPanel.OnLearnSkill(1);
                break;
            case TotemElement.air:
                tornadoLearned = true;
                playerManager.tornadoIcon.SetActive(true);
                CanvasManager.instance.skillPanel.OnLearnSkill(2);
                break;
            case TotemElement.water:
                iceSpikeLearned = true;
                playerManager.iceSpikeIcon.SetActive(true);
                CanvasManager.instance.skillPanel.OnLearnSkill(3);
                break;
        }
    }

}
