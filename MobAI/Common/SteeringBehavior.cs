using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteeringBehavior
{


    static public void Move(MobAI data)
    {
        MobAvoid(data);
        if (data.m_bMove == false)
        {
            data.AnimatorSetFloat("moveAmount", 0);
            return;
        }
        Transform t = data.m_Go.transform;
        Vector3 cPos = t.position;
        Vector3 vR = t.right;
        Vector3 vOriF = t.forward;
        Vector3 vF = data.m_vCurrentVector;

        if (data.m_fTempTurnForce > data.m_fMaxRot)
        {
            data.m_fTempTurnForce = data.m_fMaxRot;
        }
        else if (data.m_fTempTurnForce < -data.m_fMaxRot)
        {
            data.m_fTempTurnForce = -data.m_fMaxRot;
        }

        vF = vF + vR * data.m_fTempTurnForce;
        vF.Normalize();
        t.forward = vF;

        data.m_Speed = data.animator.GetFloat("moveAmount") * data.m_fMaxSpeed;

        if (data.mobtype == MobType.dragon)
        {
            Debug.Log($"dragon move speed = {data.m_Speed} move amount = {data.animator.GetFloat("moveAmount")} max speed= {data.m_fMaxSpeed} ");
        }


        if (data.m_Speed < 0.01f)
        {
            data.m_Speed = 0.01f;
        }
        else if (data.m_Speed > data.m_fMaxSpeed)
        {
            data.m_Speed = data.m_fMaxSpeed;
        }

        if (data.m_isFrozen)
        {
            Debug.Log($"{data.gameObject.name} is frozen");
            data.m_Speed *= 0.3f;
        }

        if (data.m_bCol == false)
        {
            if (SteeringBehavior.CheckCollision(data))
            {
                t.forward = vOriF;
            }
        }
        else
        {
            if (data.m_Speed < 0.02f)
            {
                if (data.m_fTempTurnForce > 0)
                {
                    t.forward = vR;
                }
                else
                {
                    t.forward = -vR;
                }

            }
        }

        cPos = cPos + t.forward * data.m_Speed;

        t.position = cPos;

        Vector3 planePos = data.transform.position;
        planePos.y = 0;
        Vector3 planePlayerPos = data.playerM.transform.position;
        planePlayerPos.y = 0;
        if (Vector3.Distance(planePos, planePlayerPos) < 2f + data.m_fRadius)
        {
            data.AnimatorSetFloat("moveAmount", 0);
        }
    }

    static public bool CheckCollision(MobAI data)       //True表示有碰撞
    {
        data.m_AvoidTargets = MobManager.instance.GetObstacles();
        if (data.m_AvoidTargets == null)
        {
            return false;
        }
        Transform ct = data.m_Go.transform;
        Vector3 cPos = ct.position;
        Vector3 cForward = ct.forward;
        Vector3 vec;

        float fDist = 0.0f;
        float fDot = 0.0f;
        int iCount = data.m_AvoidTargets.Count;
        for (int i = 0; i < iCount; i++)
        {
            vec = data.m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;
            fDist = vec.magnitude;
            if (fDist > data.m_fProbeLength + data.m_AvoidTargets[i].m_fRadius)
            {
                data.m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }

            vec.Normalize();
            fDot = Vector3.Dot(vec, cForward);
            if (fDot < 0)
            {
                data.m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }
            data.m_AvoidTargets[i].m_eState = Obstacle.eState.INSIDE_TEST;
            float fProjDist = fDist * fDot;
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > data.m_AvoidTargets[i].m_fRadius + data.m_fRadius)
            {
                continue;
            }

            return true;
        }
        return false;
    }


    static public bool CollisionAvoid(MobAI data)       //True表示需要迴避
    {
        List<Obstacle> m_AvoidTargets = MobManager.instance.GetObstacles();
        Transform ct = data.m_Go.transform;
        Vector3 cPos = ct.position;
        Vector3 cForward = ct.forward;
        data.m_vCurrentVector = cForward;
        Vector3 vec;
        float fFinalDotDist;
        float fFinalProjDist;
        Vector3 vFinalVec = Vector3.forward;
        Obstacle oFinal = null;
        float fDist = 0.0f;
        float fDot = 0.0f;
        float fFinalDot = 0.0f;
        int iCount = m_AvoidTargets.Count;

        float fMinDist = 10000.0f;
        for (int i = 0; i < iCount; i++)
        {
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;
            fDist = vec.magnitude;
            if (fDist > data.m_fProbeLength + m_AvoidTargets[i].m_fRadius)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }

            vec.Normalize();
            fDot = Vector3.Dot(vec, cForward);
            if (fDot < 0)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }
            else if (fDot > 1.0f)
            {
                fDot = 1.0f;
            }
            m_AvoidTargets[i].m_eState = Obstacle.eState.INSIDE_TEST;
            float fProjDist = fDist * fDot;
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > m_AvoidTargets[i].m_fRadius + data.m_fRadius)
            {
                continue;
            }

            if (fDist < fMinDist)
            {
                fMinDist = fDist;
                fFinalDotDist = fDotDist;
                fFinalProjDist = fProjDist;
                vFinalVec = vec;
                oFinal = m_AvoidTargets[i];
                fFinalDot = fDot;
            }

        }

        if (oFinal != null)
        {
            Vector3 vCross = Vector3.Cross(cForward, vFinalVec);
            float fTurnMag = Mathf.Sqrt(1.0f - fFinalDot * fFinalDot);
            if (vCross.y > 0.0f)
            {
                fTurnMag = -fTurnMag;
            }
            data.m_fTempTurnForce = fTurnMag;

            float fTotalLen = data.m_fProbeLength + oFinal.m_fRadius;
            float fRatio = fMinDist / fTotalLen;
            if (fRatio > 1.0f)
            {
                fRatio = 1.0f;
            }
            fRatio = 1.0f - fRatio;
            data.m_fMoveForce = -fRatio;
            oFinal.m_eState = Obstacle.eState.COL_TEST;
            data.m_bCol = true;
            data.m_bMove = true;
            return true;
        }
        data.m_bCol = false;
        return false;
    }

    static public void MobAvoid(MobAI data)       //True表示需要迴避
    {
        List<GameObject> m_AvoidMobs = MobManager.instance.GetMobs();
        float safeDistance = 0.5f;
        Vector3 vec;

        for (int i = 0; i < m_AvoidMobs.Count; i++)
        {
            MobAI mob = m_AvoidMobs[i].GetComponent<MobAI>();
            if (mob != data && mob.gameObject.activeSelf)
            {
                vec = m_AvoidMobs[i].transform.position - data.m_Go.transform.position;
                if (vec.magnitude < mob.m_fRadius + data.m_fRadius + safeDistance)
                {
                    vec.Normalize();
                    Vector3 vCross = Vector3.Cross(data.m_Go.transform.forward, vec);
                    float vDot = Vector3.Dot(vec, data.m_Go.transform.forward);
                    float fTurnMag = Mathf.Sqrt(1.0f - vDot * vDot);
                    if (vCross.y > 0.0f)
                    {
                        fTurnMag = -fTurnMag;
                    }
                    data.m_fTempTurnForce = fTurnMag;
                    data.m_bMove = true;
                    Debug.Log($"this mob {data.gameObject.name} avoid mob = {mob.name}");
                }
            }
        }

    }

    static public void PlayerAvoid(MobAI data)       //True表示需要迴避
    {
        GameObject player = PlayerManager.instance.transform.gameObject;
        float safeDistance = 2f;
        Vector3 vec;

        vec = player.transform.position - data.m_Go.transform.position;
        if (vec.magnitude < data.m_fRadius + safeDistance)
        {
            data.m_vTargetPosition = data.transform.position;
            data.m_bMove = true;
        }
    }

    static public bool Seek(MobAI data)
    {
        Vector3 cPos = data.m_Go.transform.position;
        cPos.y = 0;
        data.m_vTargetPosition.y = 0;
        Vector3 vec = data.m_vTargetPosition - cPos;
        vec.y = 0.0f;
        float fDist = vec.magnitude;
        if (fDist < data.m_Speed + 0.001f)
        {
            Vector3 vFinal = data.m_vTargetPosition;
            vFinal.y = cPos.y;
            data.m_Go.transform.position = vFinal;
            data.m_fMoveForce = 0.0f;
            data.m_fTempTurnForce = 0.0f;
            data.m_Speed = 0.0f;
            data.m_bMove = false;
            return false;
        }
        Vector3 vf = data.m_Go.transform.forward;
        Vector3 vr = data.m_Go.transform.right;
        data.m_vCurrentVector = vf;
        vec.Normalize();
        float fDotF = Vector3.Dot(vf, vec);
        if (fDotF > 0.96f)
        {
            fDotF = 1.0f;
            data.m_vCurrentVector = vec;
            data.m_fTempTurnForce = 0.0f;
            data.m_fRot = 0.0f;
        }
        else
        {
            if (fDotF < -1.0f)
            {
                fDotF = -1.0f;
            }
            float fDotR = Vector3.Dot(vr, vec);

            if (fDotF < 0.0f)
            {

                if (fDotR > 0.0f)
                {
                    fDotR = 1.0f;
                }
                else
                {
                    fDotR = -1.0f;
                }

            }
            if (fDist < 3.0f)
            {
                fDotR *= (fDist / 3.0f + 1.0f);
            }
            data.m_fTempTurnForce = fDotR;
        }

        if (fDist < 3.0f)
        {
            if (data.m_Speed > 0.1f)
            {
                data.m_fMoveForce = -(1.0f - fDist / 3.0f) * 5.0f;
            }
            else
            {
                data.m_fMoveForce = fDotF * 100.0f;
            }

        }
        else
        {
            data.m_fMoveForce = 100.0f;
        }

        data.m_bMove = true;
        return true;
    }
}
