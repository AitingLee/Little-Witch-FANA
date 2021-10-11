using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDragon : MonoBehaviour
{
    public MobAI m_mobAI;
    FSMSystem m_FSM;

    [Header("FX")]
    public ParticleSystem tailFX;   //尾巴攻擊
    public GameObject stumpFX;  //踏地攻擊
    public ParticleSystem dashWindFX;  //衝刺
    public ParticleSystem headhitFX;  //撞擊擊中
    public ParticleSystem sprayFX;  //火焰攻擊
    public GameObject fireFX; //火球攻擊
    public Transform fireBallPoint;

    [Header("Attack Power Rate")]
    public float m_fTailRate;
    public float m_fStumpRate;
    public float m_fHeadRate;
    public float m_fBreathRate;
    public float m_fFireRate;

    [Header("Attack Range")]
    public float m_fNormalAttackRange;
    public float m_fTailRange;
    public float m_fStumpRange;
    public float m_fHeadRange;
    public float m_fBreathRange;
    public float flyHeight;

    [Header("Attack Span")]
    public float breathSpan;
    public float airFireSpan;

    public Animator eyesAnimator;

    private float m_fCurrentAttackNo;
    private bool breath, dash,landing;
    private bool m_handleBestSkill, bestSkill_1, bestSkill_2;

    private float dashTimer;


    private void Start()
    {
        OnEnable();
    }
    void OnEnable()
    {
        m_mobAI = GetComponent<MobAI>();
        m_FSM = new FSMSystem(m_mobAI);
        m_mobAI.m_FSMSystem = m_FSM;

        FSMIdleState idleState = new FSMIdleState();
        FSMBattleState battleState = new FSMBattleState();
        FSMDeadState deadState = new FSMDeadState();

        idleState.AddTransition(eFSMTransition.GO_Battle, battleState);

        battleState.AddTransition(eFSMTransition.GO_Idle, idleState);

        m_FSM.AddGlobalTransition(eFSMTransition.Go_Dead, deadState);
        deadState.AddTransition(eFSMTransition.GO_Idle, idleState);


        Debug.Log("add state");
        m_FSM.AddState(idleState);
        m_FSM.AddState(battleState);
        m_FSM.AddState(deadState);

        FireSpray FS = sprayFX.GetComponent<FireSpray>();
        FS.attackPower = m_mobAI.m_fAttackPower;
        FS.sprayRate = m_fBreathRate;
    }

    void Update()
    {
        Debug.Log($"dragon Current state = {m_FSM.CurrentStateID}");
        if (!m_mobAI.m_isInteracting)
        {
            m_FSM.DoState();

            CheckBestSkill();
            if (m_handleBestSkill)
            {
                if (m_fCurrentAttackNo != 6)
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 0);
                    m_mobAI.AnimatorSetBool("isAttacking", true);
                    m_mobAI.AnimatorSetInt("attackType", 6);
                    m_mobAI.AnimatorSetBool("airFire", false);
                    m_mobAI.AnimatorSetBool("landed", false);
                    m_fCurrentAttackNo = 6;
                    AudioManager.instance.flap.Play();
                }
            }
            else if (m_mobAI.m_bHandleAttack)
            {
                m_mobAI.m_bHandleAttack = false;
                HandleAttack();
            }

            CheckSleep();


            Debug.Log($" m_fCurrentAttackNo = {m_fCurrentAttackNo}");
            if (m_fCurrentAttackNo != 0)
            {
                PerformAttack();
            }
            if (dash)
            {
                Dash();
            }
        }

        if (m_fCurrentAttackNo != 4)
        {
            if (dashWindFX.gameObject.activeSelf)
            {
                dashWindFX.gameObject.SetActive(false);
            }
        }
        
        if (m_fCurrentAttackNo != 5)
        {
            if (sprayFX.gameObject.activeSelf)
            {
                sprayFX.gameObject.SetActive(false);
            }
        }


        if (m_mobAI.m_fHp <= 0)
        {
            m_FSM.PerformGlobalTransition(eFSMTransition.Go_Dead);
            return;
        }


    }

    private void HandleAttack()
    {
        if (m_mobAI.animator.GetBool("isAttacking"))
        {
            return;
        }

        float randomNO = UnityEngine.Random.Range(1, 11);
        if (randomNO / 10 <= m_mobAI.m_skillProbability)
        {
            //放技能
            m_fCurrentAttackNo = UnityEngine.Random.Range(2, 6);
            m_mobAI.m_skillProbability = 0.3f;
        }
        else
        {
            //普通攻擊
            m_fCurrentAttackNo = 1;
        }

    }

    private void CheckSleep()
    {
        if (m_mobAI.animator.GetCurrentAnimatorStateInfo(0).IsTag("Sleep"))
        {
            m_fCurrentAttackNo = 0;
            m_mobAI.AnimatorSetFloat("moveAmount", 0);
            m_mobAI.AnimatorSetBool("isAttacking", false);
            m_mobAI.AnimatorSetInt("attackType", 0);
            m_mobAI.AnimatorSetBool("airFire", false);
            m_mobAI.AnimatorSetBool("finishBreath", true);
            m_mobAI.AnimatorSetBool("landed", true);
        }
    }
    private void CheckBestSkill()
    {
        /*
        大招流程 CheckBestSkill在update檢查血量若觸發條件則m_handleBestSkill = true
        觸發時以bestSkill_1跟bestSkill_2紀錄已經觸發過的大招次數
        update後面再檢查若m_handleBestSkill = true 則不會再處理
        若m_fCurrentAttackNo != 6 則視為剛進入大招狀態 設定 moveAmount = 0 ; isAttacking = true  ; attackType = 6 ; m_fCurrentAttackNo = 6 ; airFire = false;
        使動畫開始撥放attack state中的fly up
        因為 m_fCurrentAttackNo = 6 會進入 Perform attack的case 6
        若offset低於15就會逐漸增加提高龍的高度位置
        達到高度時開始撥放動畫fire 並且設定10秒後Invoke FinishFire 
        動畫持續loop並在適當位置呼叫FireAttack功能發射特效
        FinishFire被呼叫後設定landing為true 並設動畫landed = false開始下降
        update檢查landing為ture就會呼叫Land
        Land檢查若高度沒有回到0 則下降 並撥Fly down動畫
        當高度回到0時設定landing = false 及 landed = true播放降落動畫 並接回basic
        將m_fCurrentAttackNo 設回 0 m_handleBestSkill 為 false ; m_bPerformAttack 為 true 使FSM中回復正常攻擊模式
        */

        if (bestSkill_1 && bestSkill_2)
        {
            return;
        }

        if (!bestSkill_1)
        {
            if (m_mobAI.m_fHp / m_mobAI.m_fMaxHP < 0.5)
            {
                m_handleBestSkill = true;
                bestSkill_1 = true;
            }
        }

        if (bestSkill_1 && ! bestSkill_2)
        {
            if (m_mobAI.m_fHp / m_mobAI.m_fMaxHP < 0.2)
            {
                m_handleBestSkill = true;
                bestSkill_2 = true;
            }
        }
    }

    private void PerformAttack()
    {
        switch (m_fCurrentAttackNo)
        {
            case 1:
                //普通攻擊
                if (m_mobAI.CheckPlayerInAttackRange(m_fNormalAttackRange))
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 0);
                    m_mobAI.AnimatorSetBool("isAttacking", true);
                    m_mobAI.AnimatorSetInt("attackType", 1);
                    m_mobAI.m_bPerformAttack = true;
                    m_fCurrentAttackNo = 0;
                }
                else
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 1f);
                }
                break;
            case 2:
                //尾巴技能
                if (m_mobAI.CheckPlayerInAttackRange(m_fTailRange))
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 0);
                    m_mobAI.AnimatorSetBool("isAttacking", true);
                    m_mobAI.AnimatorSetInt("attackType", 2);
                    m_mobAI.m_bPerformAttack = true;
                    m_fCurrentAttackNo = 0;
                }
                else
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 1f);
                }
                break;
            case 3:
                //地震技能
                if (m_mobAI.CheckPlayerInAttackRange(m_fStumpRange))
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 0);
                    m_mobAI.AnimatorSetBool("isAttacking", true);
                    m_mobAI.AnimatorSetInt("attackType", 3);
                    m_mobAI.m_bPerformAttack = true;
                    m_fCurrentAttackNo = 0;
                }
                else
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 1f);
                }
                break;
            case 4:
                //衝刺技能

                m_mobAI.AnimatorSetBool("isAttacking", true);
                m_mobAI.AnimatorSetInt("attackType", 4);

                if (m_mobAI.CheckPlayerInAttackRange(m_fHeadRange))
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 0);
                    m_mobAI.AnimatorSetBool("dash", false);
                    m_mobAI.m_bPerformAttack = true;
                    m_fCurrentAttackNo = 0;
                    dashWindFX.Clear();
                    dashWindFX.gameObject.SetActive(false);
                }
                else
                {
                    if (!dash)
                    {
                        dash = true;
                        m_mobAI.AnimatorSetBool("dash", true);

                        m_mobAI.m_vTargetPosition = m_mobAI.playerM.transform.position;
                        //LookAtPlayer();
                        m_mobAI.AnimatorSetFloat("moveAmount", 1.5f);

                        dashWindFX.gameObject.SetActive(true);
                        dashTimer = 0;
                    }
                    else
                    {
                        dashTimer += Time.deltaTime;
                        if (dashTimer > 3f)
                        {
                            dash = false;
                            m_mobAI.AnimatorSetFloat("moveAmount", 0);
                            m_mobAI.AnimatorSetBool("dash", false);
                            m_mobAI.m_bPerformAttack = true;
                            m_fCurrentAttackNo = 0;
                            dashWindFX.gameObject.SetActive(false);
                            dashTimer = 0;
                        }
                    }
                }
                break;
            case 5:
                //火柱技能
                if (!breath)
                {
                    if (m_mobAI.CheckPlayerInAttackRange(m_fBreathRange))
                    {
                        m_mobAI.AnimatorSetFloat("moveAmount", 0);
                        m_mobAI.AnimatorSetBool("isAttacking", true);
                        m_mobAI.AnimatorSetInt("attackType", 5);
                        m_mobAI.AnimatorSetBool("finishBreath", false);
                        breath = true;
                        Invoke("FinishBreath", breathSpan);
                    }
                    else
                    {
                        m_mobAI.m_vTargetPosition = m_mobAI.playerM.transform.position;
                        m_mobAI.AnimatorSetFloat("moveAmount", 0.5f);
                    }
                }

                break;
            case 6:
                //空中連續火球
                if(!landing)
                {
                    if (!m_mobAI.animator.GetBool("airFire"))
                    {
                        if (m_mobAI.m_fGroundOffset < flyHeight)
                        {
                            m_mobAI.m_fGroundOffset += 0.2f;
                        }
                        else
                        {
                            m_mobAI.AnimatorSetBool("airFire", true);
                            Invoke("FinishAirFire", airFireSpan);
                        }
                    }
                }
                else
                {
                    Land();
                }
                break;
            default:
                Debug.Log("Wrong Attack No");
                break;
        }
    }



    private void NormalAttack()
    {
        m_mobAI.m_skillProbability += 0.05f;
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        float angle = Vector3.Angle(transform.forward, playerDir);
        if (angle < 20f && angle > -20f && distance < m_fNormalAttackRange + 2f)
        {
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower));
            AudioManager.instance.bite.Play();
        }
        else
        {
            Debug.Log("Normal Attack Miss");
        }
    }

    private void TailAttack()
    {
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fTailRange)
        {
            LookAtPlayer();
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower * m_fTailRate));
            AudioManager.instance.tail.Play();
        }
        if (!tailFX.gameObject.activeSelf)
        {
            tailFX.gameObject.SetActive(true);
        }

        tailFX.Clear();
        tailFX.Play();

    }

    private void StumpAttack()
    {
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fStumpRange)
        {
            LookAtPlayer();
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower * m_fStumpRate));
        }
        stumpFX = ObjectPoolManager.instance.GetFireLand();
        RaycastHit hit;
        Ray ray = new Ray(transform.position + transform.forward * 2f + Vector3.up * 3, Vector3.down);
        if (Physics.Raycast(ray, out hit, 50f, MobManager.instance.m_groundLayer))
        {
            stumpFX.transform.position = transform.position + transform.forward * 5f + hit.normal * 0.2f;
            stumpFX.transform.forward = hit.normal;
        }
        FireLand FL = stumpFX.GetComponent<FireLand>();
        FL.fireRate = m_fStumpRate;
        FL.attackPower = m_mobAI.m_fAttackPower;
        stumpFX.SetActive(true);
        AudioManager.instance.stump.Play();
    }

    private void HeadAttack()
    {
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fHeadRange)
        {
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower * m_fHeadRate));
        }

        headhitFX.transform.position = m_mobAI.playerM.transform.position + m_mobAI.playerM.transform.forward * 0.5f + m_mobAI.playerM.transform.up * 0.5f;

        if (!headhitFX.gameObject.activeSelf)
        {
            headhitFX.gameObject.SetActive(true);
        }

        headhitFX.Clear();
        headhitFX.Play();
        AudioManager.instance.headHit.Play();
    }

    private void Dash()
    {
        LookAtPlayer();
        dashWindFX.gameObject.SetActive(true);
    }

    private void SprayAttack()
    {
        if (!sprayFX.gameObject.activeSelf)
        {
            sprayFX.gameObject.SetActive(true);
        }
        if (!sprayFX.isPlaying)
        {
            sprayFX.Clear();
            sprayFX.Play();
           AudioManager.instance.fireSpray.Play();
        }
    }


    private void FinishBreath()
    {
        m_mobAI.AnimatorSetBool("finishBreath", true);
        sprayFX.gameObject.SetActive(false);
        m_fCurrentAttackNo = 0;
        m_mobAI.m_bPerformAttack = true;
        breath = false;
        AudioManager.instance.fireSpray.Stop();
    }


    public void FireAttack(int i)
    {
        Vector3 lookPos = m_mobAI.playerM.transform.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos, Vector3.up);

        FireBall(PlayerManager.instance.transform.position + Vector3.up * 1.5f);
        FireBall(PlayerManager.instance.transform.position + PlayerManager.instance.transform.forward * 3f + Vector3.up * 1.5f);
        FireBall(PlayerManager.instance.transform.position - PlayerManager.instance.transform.forward * 3f + Vector3.up * 1.5f);

    }

    public void FireBall(Vector3 targetPoint)
    {
        GameObject fireBallGO = ObjectPoolManager.instance.GetFireBall();
        FireBall fireBall = fireBallGO.GetComponent<FireBall>();
        fireBall.power = Mathf.RoundToInt(m_mobAI.m_fAttackPower * m_fFireRate);
        fireBallGO.transform.position = fireBallPoint.position;
        fireBall.transform.LookAt(targetPoint);
        fireBallGO.SetActive(true);
        Debug.Log("Fire Ball Attack");
    }

    private void FinishAirFire()
    {
        Debug.Log("Finish Air Fire");
        m_mobAI.AnimatorSetBool("airFire", false);
        landing = true;
    }

    private void Land()
    {
        if (m_mobAI.m_fGroundOffset > 0.2f)
        {
            m_mobAI.m_fGroundOffset -= 0.2f;
        }
        else
        {
            m_mobAI.m_fGroundOffset = 0;
            landing = false;
            m_mobAI.AnimatorSetBool("landed", true);
            m_fCurrentAttackNo = 0;
            m_handleBestSkill = false;
            m_mobAI.m_bPerformAttack = true;
            AudioManager.instance.flap.Stop();
        }

    }


    public void LookAtPlayer()
    {
        Vector3 lookPos = m_mobAI.playerM.transform.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos, Vector3.up);
    }

    public void Eyes(int type)
    {
        Debug.Log("Animation call Eyes");
        eyesAnimator.SetInteger("Eyes", type);
    }

    public void PlaySound(string name)
    {

    }

    public void ClearState()
    {
        Debug.Log($"Clear State");
        m_mobAI.AnimatorSetBool("landed", true);
        m_mobAI.AnimatorSetBool("finishBreath", true);
        m_mobAI.AnimatorSetBool("airFire", false);
        m_mobAI.AnimatorSetBool("isAttacking", false);
        m_mobAI.AnimatorSetInt("attackType", 0);
        m_fCurrentAttackNo = 0;
        sprayFX.gameObject.SetActive(false);
        dashWindFX.gameObject.SetActive(false);
    }


}
