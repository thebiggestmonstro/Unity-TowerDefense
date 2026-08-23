using UnityEngine;

public class Player_Castle : MonoBehaviour, IUnitInterface
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy_Base>().DestroyEnemy();
            GameEvents.RaiseEnemyReachedCastle();
        }
    }

    protected virtual void Start()
    {
        if (UnitManager.Instance != null && !UnitManager.Instance.FindContainsUnit(this))
        {
            UnitManager.Instance.RegisterUnit(this);
        }
    }

    protected virtual void OnEnable()
    {
        UnitManager.Instance?.RegisterUnit(this);
    }

    protected virtual void OnDisable()
    {
        UnitManager.Instance?.UnregisterUnit(this);
    }

    public string UnitName => gameObject.name;
}
