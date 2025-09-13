using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player : Entity
{
    public static Player instance;
    public static event Action OnPlayerDeath;

    public UI ui { get; private set; }
    public PlayerInputSet input { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX vfx { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }
    public Player_Combat combat { get; private set; }
    public Inventory_Player inventory { get; private set; }
    public Player_Stats stats { get; private set; }
    public Player_QuestManager questManager { get; private set; } 
    #region State Variables
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_jumpState jumpState { get; private set; }
    public Player_fallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttactState basicAttactState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadstate { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_SwordThrowState swordThrowState { get; private set; }
    public Player_DomainExpanslonState domainExpanslonState { get; private set; }
    #endregion

    [Header("Attack details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDurantion = 0.1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;

    [Header("Ultimate ability details")]
    public float riseSpeed = 25;
    public float riseMaxDistance = 3;


    [Header("Movement details")]
    public float moveSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpForce;
    [Range(0, 1)]
    public float inAirMoveMultiplier = .7f;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = 0.7f;
    [Space]
    public float dashDuration = 1f;
    public float dashSpeed = 20;


    public Vector2 moveInput { get; private set; }

    public Vector2 mousePosition { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        instance = this;

        ui=FindAnyObjectByType<UI>();
        skillManager = GetComponent<Player_SkillManager>();
        vfx = GetComponent<Player_VFX>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        health = GetComponent<Entity_Health>();
        combat = GetComponent<Player_Combat>();
        inventory = GetComponent<Inventory_Player>();
        stats = GetComponent<Player_Stats>();
        questManager = GetComponent<Player_QuestManager>();

        input = new PlayerInputSet();
        ui.SetupControlsUI(input);

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_jumpState(this, stateMachine, "jumpFall");
        fallState = new Player_fallState(this, stateMachine, "jumpFall");//»ìºÏÊ÷
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttactState = new Player_BasicAttactState(this, stateMachine, "basicAttact");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadstate = new Player_DeadState(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        swordThrowState = new Player_SwordThrowState(this, stateMachine, "swordThrow");
        domainExpanslonState = new Player_DomainExpanslonState(this, stateMachine, "jumpFall");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public void TeleportPlayer(Vector3 position) => transform.position = position;

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        Vector2 originalWallJump = wallJumpForce;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = attackVelocity;

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed = moveSpeed * speedMultiplier;
        jumpForce = jumpForce * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        wallJumpForce = wallJumpForce * speedMultiplier;
        jumpAttackVelocity = jumpAttackVelocity * speedMultiplier;


        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = attackVelocity[i] * speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalWallJump;
        jumpAttackVelocity=originalJumpAttack;

        for(int i = 0;i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalJumpAttack;
        }
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadstate);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Spell.performed += ctx => skillManager.shard.TryUseSkill();
        input.Player.Spell.performed += ctx => skillManager.timeEcho.TryUseSkill();

        input.Player.Interact.performed += ctx => TryInteract();

        input.Player.QuickItemSlot1.performed += ctx => inventory.TryUseQuickItemInSlot(1);
        input.Player.QuickItemSlot2.performed += ctx => inventory.TryUseQuickItemInSlot(2);


    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
        {
            StopCoroutine(queuedAttackCo);
        }
        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttactState);
    }

    private void TryInteract()
    {
        Transform closest = null;
        float closestDistnce = Mathf.Infinity;
        Collider2D[] objectsAround = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        foreach(var target in objectsAround)
        {
            IInteractable interactable=target.GetComponent<IInteractable>();
            if(interactable == null) continue;

            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < closestDistnce)
            {
                closestDistnce = distance;
                closest = target.transform;
            }
        }
        if(closest == null)
            return;

        closest.GetComponent<IInteractable>().Interact();
    }
}
