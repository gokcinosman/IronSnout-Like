using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public float damage = 10;


    void Update()
    {
        Attack();
    }
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            AnimationManager.instance.ChangeState(AnimationManager.instance.RIGHTHOOK);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            AnimationManager.instance.ChangeState(AnimationManager.instance.LEFTHOOK);

        }

    }
}
