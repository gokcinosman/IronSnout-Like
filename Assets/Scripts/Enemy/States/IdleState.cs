using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public void EnterState(IEnemy enemy)
    {
        enemy.MoveBehaviour();
    }

    public void UpdateState(IEnemy enemy)
    {
        enemy.MoveBehaviour();
    }

    public void ExitState(IEnemy enemy)
    {
        // Do nothing
    }

}
