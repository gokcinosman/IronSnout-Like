using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class DefaultEnemy : BaseEnemy
{
    public bool canMove = true;
    public int health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int damage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float speed { get; set; } = 5f;
    public float attackSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float attackRange { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    private Animator animator;
    private bool isCurrentlyWalking = false;
    private int hitCount = 0;
    private bool isKnockedBack = false;
    private float knockbackDuration = 0.5f;
    private float knockbackForce = 3f;
    private float recoveryTime = 1f;
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    public override void TakingHit()
    {
        hitCount++;
        if (hitCount >= 3)
        {
            // Knockback uygula
            StartCoroutine(KnockbackRoutine());
            hitCount = 0; // Sayacı sıfırla
        }
        else
        {
            // Normal hit recovery
            StartCoroutine(HitRecoveryRoutine());
        }
    }
    private IEnumerator KnockbackRoutine()
    {
        isKnockedBack = true;
        canMove = false;
        animator.SetBool("isKnockedBack", true);
        // Knockback yönünü hesapla (düşmanın tersi yönde)
        Vector2 knockbackDirection = (transform.position - playerTransform.position).normalized;
        // Knockback kuvvetini uygula
        float elapsedTime = 0;
        Vector2 startPos = transform.position;
        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector2.Lerp(startPos,
                startPos + (knockbackDirection * knockbackForce),
                elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Recovery süresi
        yield return new WaitForSeconds(recoveryTime);
        isKnockedBack = false;
        canMove = true;
        animator.SetBool("isKnockedBack", false);
        ChangeState(new WalkingState());
    }
    private IEnumerator HitRecoveryRoutine()
    {
        canMove = false;
        animator.SetTrigger("hit");
        yield return new WaitForSeconds(0.5f); // Kısa recovery süresi
        canMove = true;
        ChangeState(new WalkingState());
    }
    protected override void Update()
    { // eğer uzaktaysa walking state'e geçer ve hareket eder.
        // eğer yeterince yakınsa attack state'e geçer ve saldırır.
        // eğer hasar alırsa TakingHit fonksiyonu çağrılır.
        // Her frame'de mesafeyi kontrol et
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        // Eğer AttackState'deyken ve oyuncu uzaklaştıysa
        if (currentState is AttackState && distanceToPlayer > stoppingDistance)
        {
            ChangeState(new WalkingState());
            isCurrentlyWalking = false; // State değişiminde reset
        }
        // State'e göre davranışları uygula
        if (currentState is WalkingState)
        {
            MoveBehaviour();
        }
        else if (currentState is AttackState)
        {
            AttackBehaviour();
        }
    }
    public override void MoveBehaviour()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > stoppingDistance)
        {
            // Hedefe doğru hareket et
            Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            // Animasyonu sadece durum değiştiğinde güncelle
            if (!isCurrentlyWalking)
            {
                Debug.Log("Starting to walk towards player");
                isCurrentlyWalking = true;
                animator.SetBool("isWalking", true);
            }
            // Düşmanın yönünü ayarla
            float direction = playerTransform.position.x - transform.position.x;
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }
        else
        {
            if (isCurrentlyWalking)
            {
                Debug.Log("Stopping walk, changing to attack state");
                isCurrentlyWalking = false;
                animator.SetBool("isWalking", false);
            }
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
