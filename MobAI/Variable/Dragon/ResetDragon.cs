using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDragon : StateMachineBehaviour
{
    public string isAttackingBool;
    public bool isAttackingStatus;
    public string attackTypeInt;
    public int defaultAttackType;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log($"Clear State");
        animator.SetBool("landed", true);
        animator.SetBool("finishBreath", true);
        animator.SetBool("airFire", false);
        animator.SetBool("isAttacking", false);
        animator.SetInteger("attackType", 0);
    }
}
