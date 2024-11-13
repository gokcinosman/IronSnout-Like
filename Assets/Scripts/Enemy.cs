
enum EnemyTypes
{
    Default,
    OneShotAndMelee,
    Flyiny,
    Crawler,
}
public interface Enemy
{
    int health { get; set; }
    int damage { get; set; }
    float speed { get; set; }
    float attackSpeed { get; set; }
    float attackRange { get; set; }

    void MoveBehaviour();
    void AttackBehaviour();


}