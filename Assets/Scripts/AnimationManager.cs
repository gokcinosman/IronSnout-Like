using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    public string IDLE = "Idle";
    public string LEFTHOOK = "LeftHook";
    public string RIGHTHOOK = "RightHook";
    public string JUMP = "Jump";
    public float animationTime = 1f;

    public Animator animator;
    public string currentState;
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1 && !animator.IsInTransition(0))
        {
            if (currentState != IDLE)
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