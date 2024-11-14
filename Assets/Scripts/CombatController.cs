using UnityEngine;

public class CombatController : MonoBehaviour
{
    public float damage = 10;
    private float comboTimer = 0f;
    private float comboTimeout = 1f;
    private int comboCount = 0;
    private bool isRightCombo = false;
    private bool isLeftCombo = false;

    void Update()
    {
        ComboTimerControl();
        Attack();
    }

    void ComboTimerControl()
    {
        if (comboCount > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboTimeout)
            {
                ResetCombo();
            }
        }
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isLeftCombo) // Sol kombo aktif değilse
            {
                isRightCombo = true;
                ExecuteRightCombo();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (!isRightCombo) // Sağ kombo aktif değilse
            {
                isLeftCombo = true;
                ExecuteLeftCombo();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            AnimationManager.instance.ChangeState(AnimationManager.instance.CROUCH);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            AnimationManager.instance.ChangeState(AnimationManager.instance.UPPERCUT);
        }
    }

    void ExecuteRightCombo()
    {
        comboTimer = 0;
        comboCount++;

        switch (comboCount)
        {
            case 1:
                AnimationManager.instance.ChangeState(AnimationManager.instance.RIGHTHOOK);
                break;
            case 2:
                AnimationManager.instance.ChangeState(AnimationManager.instance.PUNCHRIGHT);

                break;
            case 3:
                AnimationManager.instance.ChangeState(AnimationManager.instance.KICKRIGHT);
                ResetCombo();
                break;
        }
    }

    void ExecuteLeftCombo()
    {
        comboTimer = 0;
        comboCount++;

        switch (comboCount)
        {
            case 1:
                AnimationManager.instance.ChangeState(AnimationManager.instance.LEFTHOOK);
                break;
            case 2:
                AnimationManager.instance.ChangeState(AnimationManager.instance.PUNCHLEFT);
                break;
            case 3:
                AnimationManager.instance.ChangeState(AnimationManager.instance.KICKLEFT);
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
}