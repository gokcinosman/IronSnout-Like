using UnityEngine;

public class CombatController : AnimatorBrain
{
    public float damage = 10;
    private float comboTimer = 0f;
    private float comboTimeout = 1f;
    private int comboCount = 0;
    private bool isRightCombo = false;
    private bool isLeftCombo = false;
    private bool isAttacking = false;
    private bool isCrouching = false;
    private const int UPPERBODY = 0;
    private const int LOWERBODY = 1;

    public static CombatController instance;

    void Start()
    {
        Initialize(GetComponent<Animator>().layerCount, Animations.IDLE, GetComponent<Animator>(), DefaultAnimation);
        SetupAnimationEvents();
    }

    void Update()
    {
        ComboTimerControl();
        if (!isAttacking)
        {
            CheckLowerAnimation();
            CheckTopAnimation();
        }

        // Crouch durumunu kontrol et
        if (isCrouching && !Input.GetKey(KeyCode.S))
        {
            isCrouching = false;
            isAttacking = false;
            SetLocked(false, UPPERBODY);
            SetLocked(false, LOWERBODY);
            Play(Animations.IDLE, UPPERBODY, false, true);
            Play(Animations.IDLE, LOWERBODY, false, true);
        }
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
                    animEvent.time = clip.length - 0.0001f;
                    clip.AddEvent(animEvent);
                }
            }
        }
    }

    private bool IsAttackAnimation(string clipName)
    {
        return clipName.Contains("Hook") ||
               clipName.Contains("Punch") ||
               clipName.Contains("Kick") ||
               clipName.Contains("Uppercut");
    }

    void OnAnimationComplete()
    {
        // Eğer çömelme durumunda değilsek normal sıfırlama işlemlerini yap
        if (!isCrouching)
        {
            isAttacking = false;
            SetLocked(false, UPPERBODY);
            SetLocked(false, LOWERBODY);

            if (!Input.anyKey)
            {
                Play(Animations.IDLE, UPPERBODY, false, true);
                Play(Animations.IDLE, LOWERBODY, false, true);
            }
        }
    }

    void ComboTimerControl()
    {
        if (comboCount > 0 && !isAttacking)
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
        if (isAttacking && !isCrouching) return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isLeftCombo && !isCrouching)
            {
                isRightCombo = true;
                isAttacking = true;
                ExecuteRightCombo(layer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (!isRightCombo && !isCrouching)
            {
                isLeftCombo = true;
                isAttacking = true;
                ExecuteLeftCombo(layer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (!isCrouching)
            {
                isAttacking = true;
                Play(Animations.UPPERCUT, layer, true, true);
            }
        }
        else if (Input.GetKey(KeyCode.S)) // GetKeyDown yerine GetKey kullanıyoruz
        {
            if (!isCrouching)
            {
                isCrouching = true;
                isAttacking = true;
                Play(Animations.CROUCH, layer, true, true);
            }
            // Crouch durumunda iken crouch animasyonunu tekrar oynat
            else
            {
                Play(Animations.CROUCH, layer, true, true);
            }
        }
        else if (!isAttacking && !isCrouching)
        {
            Play(Animations.IDLE, layer, false, true);
        }
    }

    void ExecuteRightCombo(int layer)
    {
        comboTimer = 0;
        comboCount++;

        switch (comboCount)
        {
            case 1:
                Play(Animations.RIGHTHOOK, layer, true, true);
                break;
            case 2:
                Play(Animations.PUNCHRIGHT, layer, true, true);
                break;
            case 3:
                Play(Animations.KICKRIGHT, layer, true, true);
                ResetCombo();
                break;
        }
    }

    void ExecuteLeftCombo(int layer)
    {
        comboTimer = 0;
        comboCount++;

        switch (comboCount)
        {
            case 1:
                Play(Animations.LEFTHOOK, layer, true, true);
                break;
            case 2:
                Play(Animations.PUNCHLEFT, layer, true, true);
                break;
            case 3:
                Play(Animations.KICKLEFT, layer, true, true);
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
}