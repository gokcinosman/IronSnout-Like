
enum EnemyTypes
{
    Default,
    OneShotAndMelee,
    Flyiny,
    Crawler,
}
public interface IEnemy
{
    int Health { get; set; }
    int Damage { get; set; }
    float Speed { get; set; }
    float AttackSpeed { get; set; }
    float AttackRange { get; set; }

    void MoveBehaviour();
    void AttackBehaviour();
    void ChangeState(IState newState);
}


