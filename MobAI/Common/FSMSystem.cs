using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    private List<FSMState> m_states;
    private Dictionary<eFSMTransition, FSMState> m_GlobalMap;
    private eFSMStateID m_currentStateID;
    public eFSMStateID CurrentStateID { get { return m_currentStateID; } }
    private FSMState m_currentState;
    public FSMState CurrentState { get { return m_currentState; } }
    private MobAI m_AI;
    public FSMSystem(MobAI mobAI)
    {
        m_states = new List<FSMState>();
        m_GlobalMap = new Dictionary<eFSMTransition, FSMState>();
        m_AI = mobAI;
    }

    public void AddGlobalTransition(eFSMTransition t, FSMState s)
    {
        m_GlobalMap.Add(t, s);
    }

    public void PerformGlobalTransition(eFSMTransition t)
    {
        if (m_GlobalMap.ContainsKey(t))
        {
            m_currentState.DoBeforeLeave(m_AI);
            m_currentState = m_GlobalMap[t];
            m_currentState.DoBeforeEnter(m_AI);
            m_currentStateID = m_currentState.m_StateID;
        }
    }

    public void AddState(FSMState s)
    {
        if (s == null)
        {
            return;
        }

        if (m_states.Count == 0)
        {
            m_states.Add(s);
            m_currentState = s;
            m_currentStateID = s.m_StateID;
            return;
        }

        foreach (FSMState state in m_states)
        {
            if (state.m_StateID == s.m_StateID)
            {
                return;
            }
        }
        m_states.Add(s);
    }

    public void DeleteState(eFSMStateID id)
    {
        if (id == eFSMStateID.NullStateID)
        {
            return;
        }

        foreach (FSMState state in m_states)
        {
            if (state.m_StateID == id)
            {
                m_states.Remove(state);
                return;
            }
        }
    }

    public void PerformTransition(eFSMTransition trans)
    {
        if (trans == eFSMTransition.NullTransition)
        {
            return;
        }

        FSMState state = m_currentState.TransitionTo(trans);
        if (state == null)
        {
            return;
        }

        // Update the currentStateID and currentState		
        m_currentState.DoBeforeLeave(m_AI);
        m_currentState = state;
        m_currentStateID = state.m_StateID;
        m_currentState.DoBeforeEnter(m_AI);

    }

    public void DoState()
    {
        if (CanvasManager.instance.freezeTime)
        {
            return;
        }
        m_currentState.CheckCondition(m_AI);
        m_currentState.Do(m_AI);
    }

}
