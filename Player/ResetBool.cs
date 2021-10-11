using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractingBool;
    public bool isInteractingStatus;

    public string isUsingRootMotionBool;
    public bool isUsingRootMotionStatus;

    public string isJumpingBool;
    public bool isJumpingStatus;

    public string isAttackingBool;
    public bool isAttackingStatus;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isUsingRootMotionBool, isUsingRootMotionStatus);
        animator.SetBool(isJumpingBool, isJumpingStatus);
        animator.SetBool(isAttackingBool, isAttackingStatus);
        animator.SetFloat("ComboStep", 0);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        animator.SetBool(isAttackingBool, isAttackingStatus);
    }

}
