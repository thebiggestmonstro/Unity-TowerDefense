using UnityEngine;

public class Area_CreatedByFan : MonoBehaviour
{
    private Player_TowerFan tower;

    private void Awake()
    {
        tower = GetComponentInParent<Player_TowerFan>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy_Base enemy))
        {
            tower.AddEnemyToReveal(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy_Base enemy))
        {
            tower.RemoveEnemyToReveal(enemy);
        }
    }
}
