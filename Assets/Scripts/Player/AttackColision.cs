
using UnityEngine;

public class AttackColision : MonoBehaviour
{
    CombatController combatController;
    void Start()
    {
        combatController = CombatController.instance;

    }

    private void OnTriggerEnter(Collider other)
    {
        KnockBack(other);


    }
    public void KnockBack(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && combatController.shouldKnockback)
        {

            Vector3 direction = new Vector3(other.gameObject.transform.position.x - transform.position.x, 0, 0);
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * 5, ForceMode.Impulse);
            other.GetComponent<BaseEnemy>().TakingHit();
        }
        else if (other.gameObject.tag == "Enemy" && combatController.isUpperCut)
        {
            Vector3 direction = new Vector3(0, transform.position.y - other.gameObject.transform.position.y, 0);
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * 5, ForceMode.Impulse);
            other.GetComponent<BaseEnemy>().TakingHit();
        }
        else
        {
            TakeDamage(other);
        }

    }
    public void TakeDamage(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Vector3 direction = new Vector3(other.gameObject.transform.position.x - transform.position.x, 0, 0);
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * 1f, ForceMode.Impulse);

        }
    }



}
