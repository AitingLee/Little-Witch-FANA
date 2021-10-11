using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    PlayerCombatMotion playerCombatMotion;
    AnimatorManager animatorManager;

    //WASD Movement
    public Vector2 movementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    //Camera Input
    public Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;
    public float zoomInput;

    //Actions
    public bool sprint_Input;
    public bool dodge_Inpudo;
    public bool attackA_Input;
    public bool Aim_Input;
    public bool attackB_Input;
    public bool jump_Input;

    //ShourtCuts
    public bool one_Input;
    public bool two_Input;
    public bool three_Input;
    public bool four_Input;
    public bool E_Input;
    public bool R_Input;
    public bool tab_Input;
    public bool M_Input;
    private void Awake()
    {
        _instance = this;
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerCombatMotion = GetComponent<PlayerCombatMotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Zoom.performed += i => zoomInput = i.ReadValue<float>();

            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;
            playerControls.PlayerActions.Dodge.performed += i => dodge_Inpudo = true;
            playerControls.PlayerActions.AttackA.performed += i => attackA_Input = true;
            playerControls.PlayerActions.AttackB.performed += i => attackB_Input = true;
            playerControls.PlayerActions.Aim.performed += i => Aim_Input = true;
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.Jump.canceled += i => jump_Input = false;

            playerControls.ShortCuts.One.performed += i => one_Input = true;
            playerControls.ShortCuts.Two.performed += i => two_Input = true;
            playerControls.ShortCuts.Three.performed += i => three_Input = true;
            playerControls.ShortCuts.Four.performed += i => four_Input = true;
            playerControls.ShortCuts.E.performed += i => E_Input = true;
            playerControls.ShortCuts.R.performed += i => R_Input = true;
            playerControls.ShortCuts.Tab.performed += i => tab_Input = true;
            playerControls.ShortCuts.M.performed += i => M_Input = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable(); 
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpInput();
        HandleDodgeInput();
        HandleAttackAInput();
        HandleAttackBInput();
        HandleSkillInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValue(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (sprint_Input && moveAmount > 0.5f && PlayerManager.instance.playerData.energy > 0)
        {
            playerLocomotion.isSprinting = true;
            PlayerManager.instance.DisplayEnergyBar(true);
        }
        else
        {
            playerLocomotion.isSprinting = false;
            sprint_Input = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jump_Input)
        {
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleDodgeInput()
    {
        if (dodge_Inpudo)
        {
            dodge_Inpudo = false;
            playerCombatMotion.HandleDodge();
        }
    }
    private void HandleAttackAInput()
    {
        if (attackA_Input)
        {
            playerCombatMotion.HandleLeftAttack();
            attackA_Input = false;
        }
    }

    private void HandleAttackBInput()
    {
        if (CanvasManager.instance.showPanel || CanvasManager.instance.freezeTime)
        {
            attackB_Input = false;
            return;
        }

        if (Aim_Input)
        {
            playerCombatMotion.HandleAim();
            Aim_Input = false;
        }

        if (attackB_Input)
        {
            playerCombatMotion.HandleRightAttack();
            attackB_Input = false;
        }


    }

    private void HandleSkillInput()
    {
        if (one_Input)
        {
            one_Input = false;
            playerCombatMotion.HandleSkill(0);  //土防護罩
        }
        if (two_Input)
        {
            two_Input = false;
            playerCombatMotion.HandleSkill(1);  //火雨
        }
        if (three_Input)
        {
            three_Input = false;
            playerCombatMotion.HandleSkill(2);  //龍捲風
        }
        if (four_Input)
        {
            four_Input = false;
            playerCombatMotion.HandleSkill(3);  //冰錐
        }
    }

    public void HandleCanvasInput()
    {
        if (tab_Input)
        {
            tab_Input = false;
            CanvasManager.instance.PressTab();
        }
        if (M_Input)
        {
            M_Input = false;
            CanvasManager.instance.DisplayMap();
        }
    }

    public void ClearAllInput()
    {
        movementInput = Vector2.zero;
        cameraInput = Vector2.zero;
        zoomInput = 0;
        sprint_Input = false;
        dodge_Inpudo = false;
        attackA_Input = false;
        attackB_Input = false;
        jump_Input = false;
        one_Input = false;
        two_Input = false;
        three_Input = false;
        four_Input = false;
        E_Input = false;
        R_Input = false;
    }

}
