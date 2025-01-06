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


    }

    public virtual void MoveBehaviour()
    {



    }
    public virtual void TakingHit()
    {


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