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
        base.Start();
    }
    protected override void Update()
    {
        // eğer uzaktaysa walking state'e geçer ve hareket eder.
        // eğer yeterince yakınsa attack state'e geçer ve saldırır.
        // eğer hasar alırsa TakingHit fonksiyonu çağrılır.
        if (currentState is WalkingState)
        {
            MoveBehaviour();
        }
        else if (currentState is AttackState)
        {
            AttackBehaviour();
        }
    }
    public override void TakingHit()
    {
        // hit animasyonu oynatılır
        // 3.kez hit aldıysa knockback olur.
        // recovery mantığı eğer knockback değilse farklı bir şekilde bekler. Eğer knockback olmuşsa knockback süresi bittiğinde walking state'e geçer.
    }
    public override void MoveBehaviour()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) > stoppingDistance)
        {
            Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 2 * Time.deltaTime);
        }
        else
        {
            ChangeState(new AttackState());
        }
    }
    public override void AttackBehaviour()
    {
        // saldırı animasyonu oynatılır
        // oynatılan animasyonda collider aktif edilir.
        // animasyondaki AttackEvent fonksiyonu çağrılır.
    }
    public void AttackEvent()
    {
        // player'a ayarlanan hasar kadar hasar verilir.
    }
}
