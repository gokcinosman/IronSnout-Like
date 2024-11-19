using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : BaseEnemy
{


    public bool canMove = true;
    public int health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int damage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float speed { get; set; } = 5f;
    public float attackSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float attackRange { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    protected override void Start()
    {

    }
    protected override void Update()
    {


    }


    public override void MoveBehaviour()
    {
        throw new System.NotImplementedException();
    }

    public override void AttackBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
