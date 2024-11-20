using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    public Transform playerTransform;
    protected IState currentState;

    public float stoppingDistance = 5f;
    public float retreatDistance = 5f;
    public int Health { get; set; }
    public int Damage { get; set; }
    public float Speed { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }

    protected virtual void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ChangeState(new WalkingState());
    }

    protected virtual void Update()
    {

        if (currentState is WalkingState)
        {
            MoveBehaviour();
        }
        else if (currentState is AttackState)
        {
            AttackBehaviour();

        }
    }

    public virtual void MoveBehaviour()
    {

        if (Vector2.Distance(transform.position, playerTransform.position) > stoppingDistance)
        {
            Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 2 * Time.deltaTime);
        }
        else
        {
            ChangeState(new AttackState());
            Debug.Log("Changing to Attack State"); // Debug için
        }

    }
    public virtual void TakingHit()
    {
        // play taking hit animation
        // if takes 3.hit, knock back
        // recovery time, wait until speed is 0
        // again walking state

    }

    public virtual void AttackBehaviour()
    {
        // play attack animation
    }

    public void ChangeState(IState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}