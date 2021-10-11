using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MobSeed : MonoBehaviour
{
    public MobAI m_mobAI;
    FSMSystem m_FSM;
    public ParticleSystem skillFX;

    [Header("Attack Rate")]
    public float m_fSkillRate;

    [Header("Attack Range")]
    public float m_fNormalAttackRange;
    public float m_fSkillRange;

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
        FSMWanderState wanderState = new FSMWanderState();
        FSMAlertState alertState = new FSMAlertState();
        FSMBattleState battleState = new FSMBattleState();
        FSMDeadState deadState = new FSMDeadState();

        idleState.AddTransition(eFSMTransition.GO_Wander, wanderState);
        idleState.AddTransition(eFSMTransition.GO_Alert, alertState);
        idleState.AddTransition(eFSMTransition.GO_Battle, battleState);

        wanderState.AddTransition(eFSMTransition.GO_Idle, idleState);
        wanderState.AddTransition(eFSMTransition.GO_Battle, battleState);
        wanderState.AddTransition(eFSMTransition.GO_Alert, alertState);

        alertState.AddTransition(eFSMTransition.GO_Battle, battleState);
        alertState.AddTransition(eFSMTransition.GO_Idle, idleState);

        battleState.AddTransition(eFSMTransition.GO_Idle, idleState);

        m_FSM.AddGlobalTransition(eFSMTransition.Go_Dead, deadState);
        deadState.AddTransition(eFSMTransition.GO_Idle, idleState);

        m_FSM.AddState(idleState);
        m_FSM.AddState(wanderState);
        m_FSM.AddState(alertState);
        m_FSM.AddState(battleState);
        m_FSM.AddState(deadState);
    }

    void Update()
    {
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

        Debug.Log($"Seed {m_FSM.CurrentStateID}");

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
        m_mobAI.m_bHandleAttack = false;
        float randomNO = UnityEngine.Random.Range(1, 11);
        if (randomNO / 10 <= m_mobAI.m_skillProbability)
        {
            m_fCurrentAttackNo = 2;
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
        switch (m_fCurrentAttackNo)
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
                    m_mobAI.m_vTargetPosition = PlayerManager.instance.transform.position;
                    m_mobAI.AnimatorSetFloat("moveAmount", 1);
                }
                break;
            case 2:
                //技能
                if (m_mobAI.CheckPlayerInAttackRange(m_fSkillRange))
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
        if (angle < 20f && angle > -20f && distance < m_fNormalAttackRange + 1f)
        {
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower));
            AudioManager.instance.normalAttack.Play();
        }
        else
        {
            Debug.Log("Normal Attack Miss");
        }

    }

    private void SkillAttack()
    {
        if (!skillFX.gameObject.activeSelf)
        {
            skillFX.gameObject.SetActive(true);
        }
        skillFX.Clear();
        skillFX.Play();
        AudioManager.instance.novaLight.Play();

        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fSkillRange + 2f)
        {
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower * m_fSkillRate));
        }
        else
        {
            Debug.Log("Skill Attack Miss");
        }
    }
}
