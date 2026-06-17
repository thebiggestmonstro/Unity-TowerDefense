using UnityEngine;

public class Player_Castle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy_Base>().TakeDamage(999);
        }
    }
}
