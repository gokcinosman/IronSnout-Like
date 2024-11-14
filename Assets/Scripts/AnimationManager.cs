using System;
using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;
    public void RegisterComboAnimation(string animationName, Action onEnd)
    {
        AnimationEventSystem.instance.RegisterEndEvent(animationName, onEnd);
    }

    public void UnregisterComboAnimation(string animationName, Action onEnd)
    {
        AnimationEventSystem.instance.UnregisterEndEvent(animationName, onEnd);
    }
    public string IDLE = "Idle";
    public string LEFTHOOK = "LeftHook";
    public string RIGHTHOOK = "RightHook";
    public string JUMP = "Jump";
    public string UPPERCUT = "Uppercut";
    public string LOWKICK = "LowKick";
    public string CROUCH = "Crouch";
    public string CROUCHPUNCHLEFT = "CrouchPunchLeft";
    public string PUNCHLEFT = "PunchLeft";
    public string PUNCHRIGHT = "PunchRight";
    public string KICKLEFT = "KickLeft";
    public string KICKRIGHT = "KickRight";

    public float animationTime = 1f;

    public Animator animator;
    public string currentState;
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 2 && !animator.IsInTransition(0))
        {
            if (currentState != IDLE && currentState != CROUCH)
            {
                ChangeState(IDLE);
            }
        }
    }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);
        currentState = newState;

    }



}