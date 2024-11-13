using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : MonoBehaviour, Enemy
{
    public float speed = 5f;
    public Transform playerTransform;
    public float stoppingDistance = 0.5f;
    public float retreatDistance = 5f;
    public bool canMove = true;

    public int health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int damage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    float Enemy.speed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float attackSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float attackRange { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        MoveToPlayerAndStop();

    }
    public void MoveToPlayerAndStop()
    {
        if (!canMove)
        {
            return;
        }
        if (Vector2.Distance(transform.position, playerTransform.position) > stoppingDistance)
        {
            Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            canMove = false;
        }

    }

    public void MoveBehaviour()
    {
        throw new System.NotImplementedException();
    }

    public void AttackBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
