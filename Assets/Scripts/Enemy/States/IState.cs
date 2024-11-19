using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void EnterState(IEnemy enemy);
    void UpdateState(IEnemy enemy);
    void ExitState(IEnemy enemy);
}

