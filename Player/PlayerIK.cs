using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    Animator animator;

    public LayerMask playerIKmask;
    public Transform headPosition;

    [Range(0, 1f)]
    public float distanceToGround;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));


            //左腳IK
            RaycastHit hitL;
            Ray rayL = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(rayL, out hitL, distanceToGround + 2f, playerIKmask))
            {
                if (hitL.transform.tag == "Walkable")
                {
                    Vector3 footPosition = hitL.point;
                    footPosition.y += distanceToGround;
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hitL.normal));
                }
            }

            //右腳IK
            RaycastHit hitR;
            Ray rayR = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(rayR, out hitR, distanceToGround + 2f, playerIKmask))
            {
                if (hitR.transform.tag == "Walkable")
                {
                    Vector3 footPosition = hitR.point;
                    footPosition.y += distanceToGround;
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hitL.normal));
                }
            }
        }
    }

    public void AdjustDieRotation()
    {
        //頭部高度
        RaycastHit hitHead;
        Ray rayHead = new Ray(headPosition.position + Vector3.up * 2, Vector3.down);
        float headHeight = 0;
        if (Physics.Raycast(rayHead, out hitHead, 10f, playerIKmask))
        {
            if (hitHead.transform.tag == "Walkable")
            {
                headHeight = hitHead.point.y;
                Debug.Log($"headHeight = {headHeight}");
            }
        }

        //根部高度
        RaycastHit hitTransform;
        Ray rayTransform = new Ray(transform.position + Vector3.up * 2, Vector3.down);
        float transformHeight = 0;
        if (Physics.Raycast(rayTransform, out hitTransform, 10f, playerIKmask))
        {
            if (hitTransform.transform.tag == "Walkable")
            {
                transformHeight = hitTransform.point.y;
                Debug.Log($"transformHeight = {transformHeight}");
            }
        }

        float height = headHeight - transformHeight; 
        float slopeDist = Vector3.Distance(headPosition.position, transform.position);
        float angle = Mathf.Asin(Mathf.Abs(height) / slopeDist) * Mathf.Rad2Deg;
        if (height > 0)
        {
            transform.localEulerAngles = new Vector3(-angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        else
        {
            transform.localEulerAngles = new Vector3(angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

    }
}
