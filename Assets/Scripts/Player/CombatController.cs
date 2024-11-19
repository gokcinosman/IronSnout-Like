using UnityEngine;

public class CombatController : AnimatorBrain
{
    public float damage = 10;
    private float comboTimer = 0f;
    private float comboTimeout = 1f;
    [SerializeField]
    private int comboCount = 0;
    private bool isRightCombo = false;
    private bool isLeftCombo = false;
    private bool isAttacking = false;
    private bool isCrouching = false;
    private const int UPPERBODY = 0;
    private const int LOWERBODY = 1;

    // Yeni eklenen değişkenler
    private bool isComboInterrupted = false;
    private float minTimeBetweenCombos = 0.1f; // Kombolar arası minimum süre
    private float lastComboTime = 0f;

    public static CombatController instance;

    void Start()
    {
        Initialize(GetComponent<Animator>().layerCount, Animations.IDLE, GetComponent<Animator>(), DefaultAnimation);
        SetupAnimationEvents();
    }

    void Update()
    {
        ComboTimerControl();
        if (!isAttacking || isComboInterrupted)
        {
            CheckLowerAnimation();
            CheckTopAnimation();
        }

        if (isCrouching && !Input.GetKey(KeyCode.S))
        {
            ResetCombatState();
        }
    }

    // Yeni eklenen method
    private void ResetCombatState()
    {
        isCrouching = false;
        isAttacking = false;
        isComboInterrupted = false;
        SetLocked(false, UPPERBODY);
        SetLocked(false, LOWERBODY);
        Play(Animations.IDLE, UPPERBODY, false, true);
        Play(Animations.IDLE, LOWERBODY, false, true);
    }

    void SetupAnimationEvents()
    {
        var animator = GetComponent<Animator>();
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (IsAttackAnimation(clip.name))
            {
                var eventExists = false;
                foreach (var evt in clip.events)
                {
                    if (evt.functionName == "OnAnimationComplete")
                    {
                        eventExists = true;
                        break;
                    }
                }

                if (!eventExists)
                {
                    AnimationEvent animEvent = new AnimationEvent();
                    animEvent.functionName = "OnAnimationComplete";
                    // Animasyon uzunluğuna göre dinamik event zamanlaması
                    float eventTime = clip.length * 0.8f; // Animasyonun %80'inde event tetiklensin
                    animEvent.time = Mathf.Min(eventTime, clip.length - 0.1f);
                    clip.AddEvent(animEvent);
                }
            }
        }
    }

    void OnAnimationComplete()
    {
        if (!isCrouching)
        {
            isAttacking = false;
            isComboInterrupted = false;
            SetLocked(false, UPPERBODY);
            SetLocked(false, LOWERBODY);

            if (!Input.anyKey && Time.time - lastComboTime >= minTimeBetweenCombos)
            {
                Play(Animations.IDLE, UPPERBODY, false, true);
                Play(Animations.IDLE, LOWERBODY, false, true);
            }
        }
    }

    void ComboTimerControl()
    {
        if (comboCount > 0 && (!isAttacking || isComboInterrupted))
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboTimeout)
            {
                ResetCombo();
            }
        }
    }

    public void Attack(int layer)
    {
        if (Time.time - lastComboTime < minTimeBetweenCombos) return;

        if (isAttacking && isCrouching) return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            HandleRightCombo(layer);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            HandleLeftCombo(layer);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (!isCrouching)
            {
                isAttacking = true;
                isRightCombo = false;
                isLeftCombo = false;
                Play(Animations.UPPERCUT, layer, true, true);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            HandleCrouch(layer);
        }
        else if (!isAttacking && !isCrouching)
        {
            Play(Animations.IDLE, layer, false, true);
        }
    }

    private void HandleRightCombo(int layer)
    {
        if (!isCrouching)
        {
            if (isAttacking && isRightCombo && !isComboInterrupted) return;

            isComboInterrupted = isAttacking && isLeftCombo;
            isRightCombo = true;
            isLeftCombo = false;
            isAttacking = true;
            ExecuteRightCombo(layer, isComboInterrupted);
        }
    }

    private void HandleLeftCombo(int layer)
    {
        if (!isCrouching)
        {
            if (isAttacking && isLeftCombo && !isComboInterrupted) return;

            isComboInterrupted = isAttacking && isRightCombo;
            isLeftCombo = true;
            isRightCombo = false;
            isAttacking = true;
            ExecuteLeftCombo(layer, isComboInterrupted);
        }
    }

    private void HandleCrouch(int layer)
    {
        if (!isCrouching)
        {
            isCrouching = true;
            isAttacking = true;
        }
        Play(Animations.CROUCH, layer, true, true);
    }

    void ExecuteRightCombo(int layer, bool interrupt)
    {
        comboTimer = 0;
        comboCount++;
        lastComboTime = Time.time;

        switch (comboCount)
        {
            case 1:
                Play(Animations.RIGHTHOOK, layer, interrupt, true);
                break;
            case 2:
                Play(Animations.PUNCHRIGHT, layer, interrupt, true);
                break;
            case 3:
                Play(Animations.KICKRIGHT, layer, interrupt, true);
                ResetCombo();
                break;
        }
    }

    void ExecuteLeftCombo(int layer, bool interrupt)
    {
        comboTimer = 0;
        comboCount++;
        lastComboTime = Time.time;

        switch (comboCount)
        {
            case 1:
                Play(Animations.LEFTHOOK, layer, interrupt, true);
                break;
            case 2:
                Play(Animations.PUNCHLEFT, layer, interrupt, true);
                break;
            case 3:
                Play(Animations.KICKLEFT, layer, interrupt, true);
                ResetCombo();
                break;
        }
    }

    void ResetCombo()
    {
        comboCount = 0;
        comboTimer = 0;
        isRightCombo = false;
        isLeftCombo = false;
        isComboInterrupted = false;
    }

    private void CheckTopAnimation()
    {
        Attack(UPPERBODY);
    }

    private void CheckLowerAnimation()
    {
        Attack(LOWERBODY);
    }

    void DefaultAnimation(int layer)
    {
        if (layer == UPPERBODY)
        {
            CheckTopAnimation();
        }
        else if (layer == LOWERBODY)
        {
            CheckLowerAnimation();
        }
    }
    private bool IsAttackAnimation(string clipName)
    {
        return clipName.Contains("Hook") ||
               clipName.Contains("Punch") ||
               clipName.Contains("Kick") ||
               clipName.Contains("Uppercut");
    }
}

