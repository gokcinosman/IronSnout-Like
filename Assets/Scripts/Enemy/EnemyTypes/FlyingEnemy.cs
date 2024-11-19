// Yeni bir düşman tipi oluşturmak için:
public class FlyingEnemy : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
        // Uçan düşmana özel başlangıç değerleri
    }

    public override void MoveBehaviour()
    {
        // Uçan düşmana özel hareket davranışı
    }

    public override void AttackBehaviour()
    {
        // Uçan düşmana özel saldırı davranışı
    }
}