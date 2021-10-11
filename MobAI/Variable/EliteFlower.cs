using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteFlower : MonoBehaviour
{
    public MobAI m_mobAI;
    FSMSystem m_FSM;
    
    [Header("FX")]
    public GameObject projectileFX;
    public GameObject roarFX;
    public Transform firePoint;

    [Header("Attack Rate")]
    public float m_fRoarRate;
    public float m_fProjectileRate;

    [Header("Attack Range")]
    public float m_fNormalAttackRange;
    public float m_fProjectileRange;
    public float m_fRoarRange;

    private float m_fCurrentAttackNo;

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
        FSMPatrolState patrolState = new FSMPatrolState();
        FSMBattleState battleState = new FSMBattleState();
        FSMDeadState deadState = new FSMDeadState();

        idleState.AddTransition(eFSMTransition.GO_Patrol, patrolState);
        idleState.AddTransition(eFSMTransition.GO_Battle, battleState);

        patrolState.AddTransition(eFSMTransition.GO_Idle, idleState);
        patrolState.AddTransition(eFSMTransition.GO_Battle, battleState);

        battleState.AddTransition(eFSMTransition.GO_Idle, idleState);

        m_FSM.AddGlobalTransition(eFSMTransition.Go_Dead, deadState);
        deadState.AddTransition(eFSMTransition.GO_Idle, idleState);


        Debug.Log("add state");
        m_FSM.AddState(idleState);
        m_FSM.AddState(battleState);
        m_FSM.AddState(deadState);
        m_FSM.AddState(patrolState);
    }

    void Update()
    {
        Debug.Log($"flower state = {m_FSM.CurrentStateID}");
        if (!m_mobAI.m_isInteracting)
        {
            m_FSM.DoState();
            if (m_mobAI.m_bHandleAttack)
            {
                m_mobAI.m_bHandleAttack = false;
                HandleAttack();
            }
            if (m_fCurrentAttackNo != 0)
            {
                PerformAttack();
            }
        }

        if (m_mobAI.m_fHp <= 0)
        {
            m_FSM.PerformGlobalTransition(eFSMTransition.Go_Dead);
            return;
        }

        if (!m_mobAI.m_bBattleBefore)
        {
            m_mobAI.m_bBattleBefore = true;
        }


    }

    private void OnDrawGizmos()
    {
        if (m_mobAI == null || m_FSM == null)
        {
            return;
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);
        if (m_FSM.CurrentStateID == eFSMStateID.IdleStateID)
        {
            Gizmos.color = Color.green;
        }
        else if (m_FSM.CurrentStateID == eFSMStateID.AlertStateID)
        {
            Gizmos.color = Color.blue;
        }
        else if (m_FSM.CurrentStateID == eFSMStateID.WanderStateID)
        {
            Gizmos.color = Color.yellow;
        }
        else if (m_FSM.CurrentStateID == eFSMStateID.BattleStateID)
        {
            Gizmos.color = Color.red;
        }
        else if (m_FSM.CurrentStateID == eFSMStateID.DeadStateID)
        {
            Gizmos.color = Color.gray;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 4f);
    }

    private void HandleAttack()
    {
        float randomNO = UnityEngine.Random.Range(1, 11);
        if (randomNO / 10 <= m_mobAI.m_skillProbability)
        {
            m_fCurrentAttackNo = UnityEngine.Random.Range(2, 4);
            Debug.Log($"施展技能, 攻擊號碼 {m_fCurrentAttackNo}");
            m_mobAI.m_skillProbability = 0.3f;
        }
        else
        {
            m_fCurrentAttackNo = 1;
            Debug.Log("施展普通攻擊");
        }
    }

    private void PerformAttack()
    {
        switch(m_fCurrentAttackNo)
        {
            case 1:
                //普通攻擊
                if (m_mobAI.CheckPlayerInAttackRange(m_fNormalAttackRange))
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 0);
                    m_mobAI.AnimatorSetBool("isAttacking", true);
                    m_mobAI.AnimatorSetFloat("attackType", 0);
                    m_mobAI.m_bPerformAttack = true;
                    m_fCurrentAttackNo = 0;
                }
                else
                {
                    m_mobAI.m_vTargetPosition = m_mobAI.playerM.transform.position;
                    m_mobAI.AnimatorSetFloat("moveAmount", 1);
                }
                break;
            case 2:
                //Projectile 技能
                if (m_mobAI.CheckPlayerInAttackRange(m_fProjectileRange))
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 0);
                    m_mobAI.AnimatorSetBool("isAttacking", true);
                    m_mobAI.AnimatorSetFloat("attackType", 0.5f);
                    m_mobAI.m_bPerformAttack = true;
                    m_fCurrentAttackNo = 0;
                }
                else
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 1);
                }
                break;
            case 3:
                //Roar 技能
                if (m_mobAI.CheckPlayerInAttackRange(m_fRoarRange))
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 0);
                    m_mobAI.AnimatorSetBool("isAttacking", true);
                    m_mobAI.AnimatorSetFloat("attackType", 1f);
                    m_mobAI.m_bPerformAttack = true;
                    m_fCurrentAttackNo = 0;
                }
                else
                {
                    m_mobAI.AnimatorSetFloat("moveAmount", 1);
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
        if (angle < 20f && angle > -20f && distance < m_fNormalAttackRange +2f)
        {
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower));
            AudioManager.instance.normalAttack.Play();
        }
        else
        {
            Debug.Log("Normal Attack Miss");
        }

    }

    private void ProjectileAttack()
    {
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fProjectileRange)
        {
            transform.LookAt(m_mobAI.playerM.transform.position , Vector3.up);
        }
        GameObject FX = Instantiate(projectileFX, firePoint.position, firePoint.rotation);
        FlowerProjectile FP = FX.GetComponent<FlowerProjectile>();
        FP.flowerAttackPower = m_mobAI.m_fAttackPower;
        FP.flowerProjectileRate = m_fProjectileRate;
        AudioManager.instance.projectile1.Play();
    }

    private void RoarAttack()
    {
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fRoarRange)
        {
            transform.LookAt(m_mobAI.playerM.transform.position, Vector3.up);
            Instantiate(roarFX, m_mobAI.playerM.transform.position + m_mobAI.playerM.transform.forward * 0.5f, Quaternion.identity);
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower * m_fRoarRate));
            AudioManager.instance.novaStrong.Play();
        }

    }

}
