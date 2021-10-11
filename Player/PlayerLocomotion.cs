using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    public Vector3 moveDirection;
    Transform cameraObject;
    public Rigidbody playerRigidbody;


    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public bool isAttacking;
    public bool isFlying;
    public bool inAimMode;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeighOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Speed")]
    public float walkingSpeed = 7f;
    public float runningSpeed = 10f;
    public float sprintingSpeed = 15;
    public float rotationSpeed = 15;

    [Header("Fly")]
    public float flyHeight = 2f;
    public float groundLevel;

    [Header("Hold")]
    public GameObject staff;
    public GameObject bloom;

    float currentSpeed = 0;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        DetectWater();
        HandleGround();

        if (inAimMode)
        {
            HandleAimModeMove();
            return;
        }
        if (playerManager.isInteracting)
            return;
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;

        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            currentSpeed = sprintingSpeed;
            PlayerManager.instance.playerData.energy -= Time.deltaTime;
            if (PlayerManager.instance.playerData.energy < 0)
            {
                PlayerManager.instance.playerData.energy = 0;
            }
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                currentSpeed = runningSpeed;
            }
            else
            {
                currentSpeed = walkingSpeed;
            }
        }
        if (CanvasManager.instance.freezeTime)
        {
            currentSpeed = 0;
        }
        playerRigidbody.velocity = moveDirection * currentSpeed;
    }

    private void HandleRotation()
    {
        if (isJumping)
            return;

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;

        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput * 0.5f;

        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleGround()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeighOffset;
        targetPosition = transform.position;

        if (!isGrounded && !isJumping && !isFlying)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            animatorManager.animator.SetBool("isUsingRootMotion", false);
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, 1.5f, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isFlying)
        {
            targetPosition.y = 1;
            transform.position = targetPosition;
        }

        if (isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void DetectWater()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position + transform.up;

        if (Physics.Raycast(rayCastOrigin, -transform.up, out hit, groundLayer))
        {
            groundLevel = hit.point.y;
        }
        if (groundLevel <= -0.5f)
        {
            animatorManager.animator.SetBool("isFlying", true);
            SwitchBloom(true);
        }
        else
        {
            if (isFlying)
            {
                SwitchBloom(false);
                animatorManager.animator.SetBool("isFlying", false);
                animatorManager.PlayTargetAnimation("Land", true);
                Vector3 rayCastHitPoint = hit.point;
                Vector3 targetPosition = transform.position;
                targetPosition.y = rayCastHitPoint.y + 0.5f;
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (isFlying || inAimMode)
        {
            return;
        }
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false, true);
            inputManager.jump_Input = false;
        }
    }


    public void SwitchBloom(bool isflying)
    {
        if (isflying)
        {
            if (staff.activeSelf)
            {
                staff.gameObject.SetActive(false);
            }
            if (!bloom.activeSelf)
            {
                bloom.gameObject.SetActive(true);
            }
        }
        else
        {
            if (!staff.activeSelf)
            {
                staff.SetActive(true);
            }
            if (bloom.activeSelf)
            {
                bloom.SetActive(false);
            }
        }
    }


    public void EnterAimMode()
    {
        inAimMode = true;
    }

    private void HandleAimModeMove()
    {
        if (isJumping)
            return;

        moveDirection = transform.forward * inputManager.verticalInput;
        moveDirection = moveDirection + transform.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        currentSpeed = walkingSpeed;

        playerRigidbody.velocity = moveDirection * currentSpeed;
    }

    public void ExitAimMode()
    {
        inAimMode = false;
    }



}
