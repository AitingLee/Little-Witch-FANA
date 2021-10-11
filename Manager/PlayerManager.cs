using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager instance
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

    public float playerRadius;
    public GameObject player;

    [Header("General UI")]
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public Slider manaBar;
    public TextMeshProUGUI manaText;
    public GameObject energyCanvas;
    public Image energyFill;

    [Header("Skill UI")]
    public GameObject tornadoIcon;
    public GameObject shieldIcon;
    public GameObject fireRainIcon;
    public GameObject iceSpikeIcon;
    [HideInInspector] public Image tornadoCDFill;
    [HideInInspector] public Image shieldCDFill;
    [HideInInspector] public Image fireRainCDFill;
    [HideInInspector] public Image iceSpikeCDFill;
    [HideInInspector] public TextMeshProUGUI tornadoCDText;
    [HideInInspector] public TextMeshProUGUI shieldCDText;
    [HideInInspector] public TextMeshProUGUI fireRainCDText;
    [HideInInspector] public TextMeshProUGUI iceSpikeCDText;


    InputManager inputManager;
    AnimatorManager animatorManager;
    Animator animator;
    public PlayerLocomotion playerLocomotion;
    public PlayerCombatMotion playerCombatMotion;
    public PlayerData playerData;

    public bool isInteracting;
    public bool isUsingRootMotion;

    [Header("Revive")]
    public float reviveTime = 5f;
    public Vector3 revivePoint;
    public RevivePoint currentRevivePoint;
    private float dieTime;

    [Header("Appear")]
    bool appearing;
    public bool disappearing;
    bool teleporting;
    public Vector3 teleportTargetPoint;
    float dissolveValue;
    public Material[] materials;
    private void Awake()
    {
        _instance = this;
        player = this.gameObject;
        playerData = new PlayerData();
        playerRadius = 1;
        
        //初始化玩家數值
        playerData.maxHP = 500;
        playerData.HP = playerData.maxHP;
        playerData.maxMP = 100;
        playerData.MP = playerData.maxMP;
        playerData.maxEnergy = 5;
        playerData.energy = playerData.maxEnergy;
        playerData.atk = 20;
        playerData.crit = 0.1f;

        //初始化UI介面參考
        tornadoCDFill = tornadoIcon.transform.GetChild(1).GetComponent<Image>();
        shieldCDFill = shieldIcon.transform.GetChild(1).GetComponent<Image>();
        fireRainCDFill = fireRainIcon.transform.GetChild(1).GetComponent<Image>();
        iceSpikeCDFill = iceSpikeIcon.transform.GetChild(1).GetComponent<Image>();
        tornadoCDText = tornadoIcon.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        shieldCDText = shieldIcon.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        fireRainCDText = fireRainIcon.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        iceSpikeCDText = iceSpikeIcon.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponent<AnimatorManager>();
        animator = GetComponent<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerCombatMotion = GetComponent<PlayerCombatMotion>();
    }

    private void Start()
    {
        currentRevivePoint.SetRecordPoint();
        UpdateHealthBar();
        UpdateManaBar();
    }

    private void Update()
    {
        // for test use

        if (Input.GetKeyDown(KeyCode.F1))
        {
            TestStartBossFight();
        }


        if (playerData.HP <= 0)
        {
            HandleDead();
        }
        if (appearing)
        {
            Appear();
            return;
        }
        else if (disappearing)
        {
            Disappear();
            return;
        }

        inputManager.HandleCanvasInput();

        if (CanvasManager.instance.freezeTime)
        {
            return;
        }

        inputManager.HandleAllInputs();
        UpdateCDDisplay();
        if (energyCanvas.activeSelf)
        {
            UpdateEnergyBar();
        }

    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        isUsingRootMotion = animator.GetBool("isUsingRootMotion");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        playerLocomotion.isAttacking = animator.GetBool("isAttacking");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
        playerLocomotion.isFlying = animator.GetBool("isFlying");
    }

    public void TakeDamage(int amount)
    {
        if (!animator.GetBool("isDead"))
        {
            if (playerCombatMotion.damageAvoid)
            {
                //TODO 跳miss
                return;
            }
            playerCombatMotion.HandleDamage();
            playerCombatMotion.DisplayDamageNum(amount, 1, transform.position + new Vector3(0, 4, 0));
            playerData.HP -= amount;
            if (playerData.HP < 0)
            {
                playerData.HP = 0;
            }
            UpdateHealthBar();
        }
    }

    private void Appear()
    {
        if (dissolveValue < 0.5f)
        {
            dissolveValue += 0.02f;
            foreach (Material m in materials)
            {
                m.SetFloat("_Clipping_Level", dissolveValue);
            }
        }
        else
        {
            foreach (Material m in materials)
            {
                m.SetFloat("_Clipping_Level", 0.5f);
            }
            CanvasManager.instance.DiaplayPlayerInfo(true);
            appearing = false;
            teleporting = false;
        }
    }

    private void Disappear()
    {
        if (dissolveValue > 0.02f)
        {
            dissolveValue -= 0.02f;
            foreach (Material m in materials)
            {
                m.SetFloat("_Clipping_Level", dissolveValue);
            }
        }
        else
        {
            foreach (Material m in materials)
            {
                m.SetFloat("_Clipping_Level", 0);
            }
            disappearing = false;
            CanvasManager.instance.DiaplayPlayerInfo(false);

            if (teleporting)
            {
                TeleportAppear();
            }
        }
    }

    public void TeleportDisappear()
    {
        teleporting = true;
        disappearing = true;
    }

    public void TeleportAppear()
    {
        transform.position = teleportTargetPoint;
        appearing = true;
    }


    public void UpdateHealthBar()
    {
        healthBar.value = (float)playerData.HP / (float)playerData.maxHP;
        healthText.text = $"{playerData.HP} / {playerData.maxHP}";
    }

    public void UpdateManaBar()
    {
        manaBar.value = (float)playerData.MP / (float)playerData.maxMP;
        manaText.text = $"{playerData.MP} / {playerData.maxMP}";
    }

    public void DisplayEnergyBar(bool display)
    {
        if (display && !energyCanvas.activeSelf)
        {
            energyCanvas.SetActive(true);
        }
        else if (!display && energyCanvas.activeSelf)
        {
            energyCanvas.SetActive(false);
        }
    }

    public void UpdateEnergyBar()
    {
        energyFill.fillAmount = playerData.energy / playerData.maxEnergy;
        if (!playerLocomotion.isSprinting)
        {
            playerData.energy += Time.deltaTime;
            if (playerData.energy > playerData.maxEnergy)
            {
                playerData.energy = playerData.maxEnergy;
                DisplayEnergyBar(false);
            }
        }
    }


    public void UpdateCDDisplay()
    {
        if (playerCombatMotion.shieldCDTime > 0)
        {
            shieldCDFill.fillAmount = playerCombatMotion.shieldCDTime / playerCombatMotion.shieldCDSpan;
            shieldCDText.text = Mathf.CeilToInt(playerCombatMotion.shieldCDTime).ToString();
        }
        else
        {
            shieldCDFill.fillAmount = 0;
            shieldCDText.text = "";
        }

        if (playerCombatMotion.fireRainCDTime > 0)
        {
            fireRainCDFill.fillAmount = playerCombatMotion.fireRainCDTime / playerCombatMotion.fireRainCDSpan;
            fireRainCDText.text = Mathf.CeilToInt(playerCombatMotion.fireRainCDTime).ToString();
        }
        else
        {
            fireRainCDFill.fillAmount = 0;
            fireRainCDText.text = "";
        }

        if (playerCombatMotion.iceSpikeCDTime > 0)
        {
            iceSpikeCDFill.fillAmount = playerCombatMotion.iceSpikeCDTime / playerCombatMotion.iceSpikeCDSpan;
            iceSpikeCDText.text = Mathf.CeilToInt(playerCombatMotion.iceSpikeCDTime).ToString();
        }
        else
        {
            iceSpikeCDFill.fillAmount = 0;
            iceSpikeCDText.text = "";
        }

        if (playerCombatMotion.tornadoCDTime > 0)
        {
            tornadoCDFill.fillAmount = playerCombatMotion.tornadoCDTime / playerCombatMotion.tornadoCDSpan;
            tornadoCDText.text = Mathf.CeilToInt(playerCombatMotion.tornadoCDTime).ToString();
        }
        else
        {
            tornadoCDFill.fillAmount = 0;
            tornadoCDText.text = "";
        }
    }


    public void HandleDead()
    {
        if (!animator.GetBool("isDead"))
        {
            animator.SetBool("isDead", true);
            dieTime = Time.time;
            AudioManager.instance.deadSound.Play();
        }
        if (Time.time - dieTime > reviveTime)
        {
            playerData.HP = playerData.maxHP;
            UpdateHealthBar();
            playerData.MP = playerData.maxMP;
            UpdateManaBar();
            animator.SetBool("isDead", false);
            transform.position = revivePoint;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            dissolveValue = 0;
            disappearing = false;
            appearing = true;
        }
    }

    public void HandleDisappear()
    {
        dissolveValue = 0.5f;
        disappearing = true;
    }
    public void AddManaValue(int value)
    {
        if (value > 0)
        {
            if (playerData.MP + value <= playerData.maxMP)
            {
                playerData.MP += value;
            }
            else
            {
                playerData.MP = playerData.maxMP;
            }
        }
        else
        {
            if (playerData.MP + value >= 0)
            {
                playerData.MP += value;
            }
            else
            {
                playerData.MP = 0;
            }
        }
        UpdateManaBar();
    }

    public void AddHealthValue(int value)
    {
        if (value > 0)
        {
            if (playerData.HP + value <= playerData.maxHP)
            {
                playerData.HP += value;
            }
            else
            {
                playerData.HP = playerData.maxHP;
            }
        }
        else
        {
            if (playerData.HP + value >= 0)
            {
                playerData.HP += value;
            }
            else
            {
                playerData.HP = 0;
            }
        }

        UpdateHealthBar();
    }

    public void InvokeFreezeTimeScale(float time)
    {
        Invoke("FreezeTimeScale", time);
    }

    public void FreezeTimeScale()
    {
        Debug.Log("Freeze Tiem Scale");
        CanvasManager.instance.ShowCuresor();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
    }


    public void DefreezeTimeScale()
    {
        Debug.Log("Defreeze Tiem Scale");
        CanvasManager.instance.HideCuresor();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
    }

    public void EquipmentValueChange(int addMaxHP, int addMaxMP, int addAtk)
    {
        playerData.maxHP += addMaxHP;
        playerData.maxMP += addMaxMP;
        playerData.atk += addAtk;
        if (playerData.HP > playerData.maxHP)
        {
            playerData.HP = playerData.maxHP;
        }
        if (playerData.MP > playerData.maxMP)
        {
            playerData.MP = playerData.maxMP;
        }

        UpdateHealthBar();
        UpdateManaBar();
    }

    public void UpgradeGem(ItemSO whichGem)
    {
        playerData.maxHP += whichGem.increaseMaxHP;
        playerData.maxMP += whichGem.increaseMaxMP;
        playerData.atk += whichGem.increaseAttack;
        UpdateHealthBar();
        UpdateManaBar();
        CanvasManager.instance.equipment.UpdatePlayerData(playerData);
    }

    #region for Test Version

    void TestStartBossFight()
    {
        playerData.maxHP = 800;
        playerData.maxMP = 250;
        playerData.HP = playerData.maxHP;
        playerData.MP = playerData.maxMP;
        UpdateHealthBar();
        UpdateManaBar();
        playerData.atk = 50;

        playerCombatMotion.LearnSpell(TotemElement.earth);
        playerCombatMotion.LearnSpell(TotemElement.fire);
        playerCombatMotion.LearnSpell(TotemElement.air);
        playerCombatMotion.LearnSpell(TotemElement.water);

        CanvasManager.instance.inventory.PutInBag(new ItemInformation(ItemName.honey, ItemManager.instance.GetItemSO(ItemName.honey), 50));
        TaskManager.instance.CanAcceptTask(TaskManager.instance.allTasks[14]);
        TaskManager.instance.ActiveBossBattle();
    }
    #endregion
}
