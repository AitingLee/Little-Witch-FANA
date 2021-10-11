using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFSMTransition
{
    NullTransition = 0,
    GO_Idle,
    GO_Wander,
    GO_Patrol,
    GO_Alert,
    GO_Battle,
    Go_Dead,

    GO_BossIdle,
    GO_BossGround,
    GO_BossAir,
    GO_BossDescent,
    GO_BossTakeOff,
}

public enum eFSMStateID
{
    NullStateID = 0,
    IdleStateID,
    WanderStateID,
    PatrolStateID,
    AlertStateID,
    BattleStateID,
    DeadStateID,

    BossIdleStateID,
    BossGroundStateID,
    BossAirStateID,
    BossDescentStateID,
    BossTakeOffStateID,
}

public class FSMState
{
    public eFSMStateID m_StateID;
    public Dictionary<eFSMTransition, FSMState> m_Map;
    public float m_fCurrentTime;

    public FSMState()
    {
        m_StateID = eFSMStateID.NullStateID;
        m_fCurrentTime = 0.0f;
        m_Map = new Dictionary<eFSMTransition, FSMState>();
    }

    public void AddTransition(eFSMTransition trans, FSMState toState)
    {
        if (m_Map.ContainsKey(trans))
        {
            return;
        }

        m_Map.Add(trans, toState);
    }
    public void DelTransition(eFSMTransition trans)
    {
        if (m_Map.ContainsKey(trans))
        {
            m_Map.Remove(trans);
        }

    }
    public FSMState TransitionTo(eFSMTransition trans)
    {
        if (m_Map.ContainsKey(trans) == false)
        {
            return null;
        }
        return m_Map[trans];
    }
    public virtual void DoBeforeEnter(MobAI data)
    {

    }
    public virtual void DoBeforeLeave(MobAI data)
    {

    }
    public virtual void Do(MobAI data)
    {

    }
    public virtual void CheckCondition(MobAI data)
    {

    }
}

public class FSMIdleState : FSMState
{
    private float m_fIdleTime;

    public FSMIdleState()
    {
        m_StateID = eFSMStateID.IdleStateID;
    }


    public override void DoBeforeEnter(MobAI data)
    {
        m_fCurrentTime = 0.0f;
        m_fIdleTime = Random.Range(1.0f, 3.0f);
        data.AnimatorSetFloat("moveAmount", 0);

        if (data.mobtype == MobType.dragon)
        {
            data.AnimatorSetBool("finishBreath", true);
            data.AnimatorSetBool("isAttacking", false);
        }
    }

    public override void DoBeforeLeave(MobAI data)
    {

    }

    public override void Do(MobAI data)
    {
        m_fCurrentTime += Time.deltaTime;

        if (!data.m_isAmbush)
        {
            Vector3 cPos = data.transform.position;
            RaycastHit hit;
            Ray ray = new Ray(cPos + Vector3.up * 10, Vector3.down);
            if (Physics.Raycast(ray, out hit, 50f, MobManager.instance.m_groundLayer))
            {
                cPos.y = hit.point.y + data.m_fGroundOffset;
            }
            data.transform.position = cPos;
        }

        if (data.animator.GetFloat("moveAmount") > 0f)
        {
            data.AnimatorSetFloat("moveAmount", 0);
        }
    }

    public override void CheckCondition(MobAI data)
    {
        if (data.m_isAmbush)
        {
            return;
        }

        if (data.CheckPlayerInSight())
        {
            if (data.m_bBattleBefore)
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Battle);
                if (data.mobtype == MobType.dragon)
                {
                    data.AnimatorSetBool("isAwake", true);
                    return;
                }
            }
            else
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Alert);
            }
        }

        if (m_fCurrentTime > m_fIdleTime)
        {
            if (data.mobtype == MobType.dragon)
            {
                data.AnimatorSetBool("isAwake", false);
            }
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Patrol);
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Wander);
        }
    }
}

public class FSMPatrolState : FSMState
{
    private int m_iCurrentPatrolPt;
    private Vector3[] m_PatrolPoints;

    public FSMPatrolState()
    {
        m_StateID = eFSMStateID.PatrolStateID;
        m_iCurrentPatrolPt = -1;
        m_PatrolPoints = new Vector3[4];
    }


    public override void DoBeforeEnter(MobAI data)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 newPoints = data.m_homePosition + new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
            m_PatrolPoints[i] = newPoints;
        }
        int iNewPt = Random.Range(0, m_PatrolPoints.Length);
        if (m_iCurrentPatrolPt != iNewPt)
        {
            m_iCurrentPatrolPt = iNewPt;
        }
        else
        {
            if (iNewPt +1 < m_PatrolPoints.Length)
            {
                iNewPt++;
            }
            else
            {
                iNewPt = 0;
            }
        }
        data.m_vTargetPosition = m_PatrolPoints[iNewPt];
        data.m_bMove = true;
        data.AnimatorSetFloat("moveAmount", 0.5f);
    }

    public override void DoBeforeLeave(MobAI data)
    {

    }

    public override void Do(MobAI data)
    {
        if (SteeringBehavior.CollisionAvoid(data) == false)
        {
            SteeringBehavior.Seek(data);
        }

        SteeringBehavior.Move(data);
    }

    public override void CheckCondition(MobAI data)
    {
        if (data.CheckPlayerInSight())
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Battle);
        }

        if (data.m_bMove == false || data.playerM.playerData.HP <= 0)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Idle);
        }
    }
}

public class FSMWanderState : FSMState
{
    private float m_fWanderTime;
    public PathPoint m_nearPoint, m_farPoint;
    public Vector3 m_midPos1, m_midPos2;
    int currentPt;
    public float m_closeDistance;

    public FSMWanderState()
    {
        m_StateID = eFSMStateID.WanderStateID;
        m_closeDistance = 5f;
    }


    public override void DoBeforeEnter(MobAI data)
    {
        m_fCurrentTime = 0.0f;
        m_fWanderTime = Random.Range(5, 15f);

        //定義near Point為最接近的Path Point
        float minDistance = 500;

        foreach (PathPoint pp in data.m_thisIslandPoints)
        {
            Vector3 planePP = pp.transform.position;
            planePP.y = 0;
            Vector3 planePos = data.transform.position;
            planePos.y = 0;
            float ppDistance = Vector3.Distance(planePP, planePos);
            if (ppDistance < minDistance && ppDistance > m_closeDistance)
            {
                m_nearPoint = pp;
                minDistance = ppDistance;
            }
        }
        //定義far Point為near Point附近點列表中的隨機一個點
        int randNo = UnityEngine.Random.Range(0, m_nearPoint.closePoint.Length);
        m_farPoint = m_nearPoint.closePoint[randNo].GetComponent<PathPoint>();

        Vector3 nearPlane = m_nearPoint.transform.position;
        nearPlane.y = 0;
        Vector3 farPlane = m_farPoint.transform.position;
        farPlane.y = 0;
        //取near Point跟far Point兩點1/3 及 2/3處 並加上隨機x, y 偏移 作為mid Point 1和2
        Vector3 vec = farPlane - nearPlane;
        Vector3 pos1 = new Vector3(nearPlane.x * 2 + farPlane.x, 0, nearPlane.z * 2 + farPlane.z) / 3;
        Vector3 pos2 = new Vector3(nearPlane.x + farPlane.x * 2, 0, nearPlane.z + farPlane.z * 2) / 3;
        m_midPos1 = pos1 + new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
        m_midPos2 = pos2 + new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));

        //將data的目標設為near Point , currentPt 為 0
        data.m_vTargetPosition = m_nearPoint.transform.position;
        currentPt = 0;
        data.m_bMove = true;
        data.AnimatorSetFloat("moveAmount", 0.5f);
    }

    public override void DoBeforeLeave(MobAI data)
    {
    }

    public override void Do(MobAI data)
    {
        if (SteeringBehavior.CollisionAvoid(data) == false)
        {
            SteeringBehavior.Seek(data);
        }

        SteeringBehavior.Move(data);

        if (data.animator.GetFloat("moveAmount") > 0.5f)
        {
            data.AnimatorSetFloat("moveAmount", 0.5f);
        }
        m_fCurrentTime += Time.deltaTime;
    }

    public override void CheckCondition(MobAI data)
    {
        Vector3 planePos = data.transform.position;
        planePos.y = 0;
        Vector3 targetPoint = Vector3.zero;

        switch (currentPt)
        {
            //如果currentPt = 0, 角色接近near Point, 將目標改為mid Point 1, currentPt 改為 1
            //如果currentPt = 1, 角色接近mid Point 1, 將目標改為mid Point 2, currentPt 改為 2
            //如果currentPt = 2, 角色接近mid Point 2, 將目標改為far Point, currentPt 改為 3
            case 0:
                targetPoint = m_nearPoint.transform.position;
                targetPoint.y = 0;
                if (Vector3.Distance(planePos, targetPoint) < m_closeDistance )
                {
                    data.m_vTargetPosition = m_midPos1;
                    currentPt = 1;
                }
                break;
            case 1:
                targetPoint = m_midPos1;
                targetPoint.y = 0;
                if (Vector3.Distance(planePos, targetPoint) < m_closeDistance)
                {
                    data.m_vTargetPosition = m_midPos2;
                    currentPt = 2;
                }
                break;
            case 2:
                targetPoint = m_midPos2;
                targetPoint.y = 0;
                if (Vector3.Distance(planePos, targetPoint) < m_closeDistance)
                {
                    data.m_vTargetPosition = m_farPoint.transform.position;
                    currentPt = 3;
                }
                break;
        }

        if (data.CheckPlayerInSight())
        {
            if (data.m_bBattleBefore)
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Battle);
            }
            else
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Alert);
            }
        }

        if (data.m_bMove == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Idle);
        }

        if (m_fCurrentTime > m_fWanderTime)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Idle);
        }
    }
}

public class FSMAlertState : FSMState
{

    private float m_fAlertTime;


    public FSMAlertState()
    {
        m_StateID = eFSMStateID.AlertStateID;
        m_fAlertTime = 5f;
    }


    public override void DoBeforeEnter(MobAI data)
    {
        m_fCurrentTime = 0.0f;
        data.AnimatorSetFloat("moveAmount", 0);
        data.DisplayQuestionMark();
    }

    public override void DoBeforeLeave(MobAI data)
    {

    }

    public override void Do(MobAI data)
    {
        m_fCurrentTime += Time.deltaTime;
        Vector3 lookPos = data.playerM.transform.position;
        lookPos.y = data.transform.position.y;
        data.transform.LookAt(lookPos, Vector3.up);
    }

    public override void CheckCondition(MobAI data)
    {
        if (data.m_bBattleBefore || m_fCurrentTime > m_fAlertTime)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Battle);
            data.AlertToAttack();
        }

        if (!data.CheckPlayerInSight())
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Idle);
            data.DisableCurrentMark();
            m_fCurrentTime = 0;
        }

    }
}

public class FSMBattleState : FSMState
{
    private float fAttackTime;
    public FSMBattleState()
    {
        m_StateID = eFSMStateID.BattleStateID;
    }


    public override void DoBeforeEnter(MobAI data)
    {
        data.AnimatorSetFloat("moveAmount", 1);
        fAttackTime = Random.Range(1f, 2f);
        m_fCurrentTime = 0.0f;
    }

    public override void DoBeforeLeave(MobAI data)
    {
        data.m_bHandleAttack = false;
    }


    public override void Do(MobAI data)
    {
        if (data.mobtype == MobType.dragon)
        {
            if (data.animator.GetCurrentAnimatorStateInfo(0).IsName("Basic") || data.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
            {
            }
            else
            {
                Debug.Log($"dragon not in basic or dash, set move 0");
                data.AnimatorSetFloat("moveAmount", 0);
            }
        }

        data.m_vTargetPosition = data.playerM.transform.position;

        m_fCurrentTime += Time.deltaTime;

        if (data.m_bPerformAttack)
        {
            m_fCurrentTime = 0;
            fAttackTime = Random.Range(1f, 3f);
            data.m_bHandleAttack = false;
            data.m_bPerformAttack = false;
        }
        else
        {
            if (m_fCurrentTime > fAttackTime)
            {
                data.m_bHandleAttack = true;
            }
            else
            {
                data.AnimatorSetFloat("moveAmount", 0);
            }
        }

        if (SteeringBehavior.CollisionAvoid(data) == false)
        {
            SteeringBehavior.Seek(data);
        }

        SteeringBehavior.Move(data);
    }

    public override void CheckCondition(MobAI data)
    {
        if (!data.CheckPlayerInBattleRange())
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Idle);
        }

        if(data.playerM.playerLocomotion.isFlying || data.playerM.playerData.HP <= 0 || data.playerM.disappearing)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Idle);
        }
    }
}

public class FSMDeadState : FSMState
{

    public FSMDeadState()
    {
        m_StateID = eFSMStateID.DeadStateID;
    }


    public override void DoBeforeEnter(MobAI data)
    {
        if (!data.m_isDead)
        {
            data.AnimatorSetBool("isDead", true);
        }
        data.m_isInteracting = false;
    }

    public override void DoBeforeLeave(MobAI data)
    {
    }

    public override void Do(MobAI data)
    {
    }

    public override void CheckCondition(MobAI data)
    {

    }
}