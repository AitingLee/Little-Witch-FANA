using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBatLord : MonoBehaviour
{
    public MobAI m_mobAI;
    FSMSystem m_FSM;

    [Header("FX")]
    public ParticleSystem spinFX;
    public ParticleSystem spellFX;

    [Header("Attack Power Rate")]
    public float m_fSpinRate;
    public float m_fSpellRate;

    [Header("Attack Range")]
    public float m_fNormalAttackRange;
    public float m_fSpinRange;
    public float m_fSpellRange;

    private float m_fCurrentAttackNo;
    private bool m_isFallingGround;
    private Vector3 diePos;
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
            m_mobAI.m_skillProbability = 0.3f;
        }
        else
        {
            m_fCurrentAttackNo = 1;
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
                    m_mobAI.m_vTargetPosition = m_mobAI.playerM.transform.position;
                    m_mobAI.AnimatorSetFloat("moveAmount", 1);
                }
                break;
            case 2:
                //Spin 技能
                if (m_mobAI.CheckPlayerInAttackRange(m_fSpinRange))
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
                //Spell 技能
                if (m_mobAI.CheckPlayerInAttackRange(m_fSpellRange))
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
        if (angle < 20f && angle > -20f && distance < m_fNormalAttackRange + 2f)
        {
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower));
            AudioManager.instance.normalAttack.Play();
        }
        else
        {
            Debug.Log("Normal Attack Miss");
        }
    }

    private void SpinAttack()
    {
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fSpinRange)
        {
            Vector3 lookPos = m_mobAI.playerM.transform.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos, Vector3.up);
        }
        if (!spinFX.gameObject.activeSelf)
        {
            spinFX.gameObject.SetActive(true);
        }
        SpinLightning SL = spinFX.GetComponent<SpinLightning>();
        SL.spinRate = m_fSpinRate;
        SL.attackPower = m_mobAI.m_fAttackPower;
        spinFX.Clear();
        spinFX.Play();
    }

    private void SpellAttack()
    {
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fSpellRange)
        {
            Vector3 lookPos = m_mobAI.playerM.transform.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos, Vector3.up);
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower * m_fSpellRate));
        }
        spellFX.transform.position = m_mobAI.playerM.transform.position + Vector3.up;
        if (!spellFX.gameObject.activeSelf)
        {
            spellFX.gameObject.SetActive(true);
        }

        spellFX.Clear();
        spellFX.Play();
        AudioManager.instance.lightingStrike.Play();
    }

    private void DieFall()
    {
        m_isFallingGround = true;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, 50f, MobManager.instance.m_groundLayer))
        {
            diePos = transform.position;
            diePos.y = hit.point.y;
        }
    }

    private void UpdateDieFall()
    {
        if (Vector3.Distance(transform.position, diePos) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, diePos, 0.2f);
        }
        else
        {
            transform.position = diePos;
            m_isFallingGround = false;
        }
    }
}
