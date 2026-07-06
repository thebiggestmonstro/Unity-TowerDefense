using UnityEngine;

public class Player_Castle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy_Base>().DestroyEnemy();
            GameManager.Instance.UpdateHp(-1);
        }
    }
}
