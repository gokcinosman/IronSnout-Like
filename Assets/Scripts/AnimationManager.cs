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

        animator.Play(newState, 0, 0);
        currentState = newState;
        // StartCoroutine(ChangeStateAfterTime(IDLE));
    }
    public IEnumerator ChangeStateAfterTime(string newState)
    {
        yield return new WaitForSeconds(animationTime);
        ChangeState(newState);
    }


}