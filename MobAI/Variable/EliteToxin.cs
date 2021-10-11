using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteToxin : MonoBehaviour
{
    public MobAI m_mobAI;
    FSMSystem m_FSM;

    [Header("FX")]
    public GameObject poisonFX;
    public ParticleSystem spellFX;

    [Header("Attack Power Rate")]
    public float m_fPoisonRate;
    public float m_fSpellRate;

    [Header("Attack Range")]
    public float m_fNormalAttackRange;
    public float m_fPoisonRange;
    public float m_fSpellRange;

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

        m_FSM.AddState(idleState);
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
            Gizmos.color = Color.yellow;
        }
        else if (m_FSM.CurrentStateID == eFSMStateID.WanderStateID || m_FSM.CurrentStateID == eFSMStateID.PatrolStateID)
        {
            Gizmos.color = Color.blue;
        }
        else if (m_FSM.CurrentStateID == eFSMStateID.BattleStateID)
        {
            Gizmos.color = Color.red;
        }
        else if (m_FSM.CurrentStateID == eFSMStateID.DeadStateID)
        {
            Gizmos.color = Color.gray;
        }
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
                //Projectile 技能
                if (m_mobAI.CheckPlayerInAttackRange(m_fPoisonRange))
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

    private void PoisonAttack()
    {
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fPoisonRange)
        {
            Vector3 lookPos = m_mobAI.playerM.transform.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos, Vector3.up);
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower * m_fPoisonRate));
        }
        poisonFX = ObjectPoolManager.instance.GetToxinPoison();
        RaycastHit hit;
        Ray ray = new Ray(transform.position + transform.forward * 2f + Vector3.up * 3, Vector3.down);
        if (Physics.Raycast(ray, out hit, 50f, MobManager.instance.m_groundLayer))
        {
            poisonFX.transform.position = transform.position + transform.forward * 2f + hit.normal *0.2f;
            poisonFX.transform.forward = hit.normal;
        }
        ToxinPoison TP = poisonFX.GetComponent<ToxinPoison>();
        TP.poisonRate = m_fPoisonRate;
        TP.attackPower = m_mobAI.m_fAttackPower;
        poisonFX.SetActive(true);
        AudioManager.instance.acid.Play();
    }

    private void SpellAttack()
    {
        Vector3 playerPlanePos = m_mobAI.playerM.transform.position;
        playerPlanePos.y = 0;
        Vector3 thisPlanePos = transform.position;
        thisPlanePos.y = 0;
        Vector3 playerDir = playerPlanePos - thisPlanePos;
        float distance = playerDir.magnitude;
        if (distance < m_fSpellRange +2f)
        {
            Vector3 lookPos = m_mobAI.playerM.transform.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos, Vector3.up);
            m_mobAI.playerM.TakeDamage(Mathf.RoundToInt(m_mobAI.m_fAttackPower * m_fSpellRate));
        }
        if (!spellFX.gameObject.activeSelf)
        {
            spellFX.gameObject.SetActive(true);
        }

        spellFX.Clear();
        spellFX.Play();
        AudioManager.instance.lightingExplosion.Play();
    }
}
