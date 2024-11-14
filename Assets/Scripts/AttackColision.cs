
using UnityEngine;

public class AttackColision : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {

            Vector3 direction = new Vector3(other.gameObject.transform.position.x - transform.position.x, 0, 0);
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);
        }
    }



}
