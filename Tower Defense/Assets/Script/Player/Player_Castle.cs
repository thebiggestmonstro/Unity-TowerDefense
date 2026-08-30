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
        if (GameServices.Get<UnitManager>() != null && !GameServices.Get<UnitManager>().FindContainsUnit(this))
        {
            GameServices.Get<UnitManager>().RegisterUnit(this);
        }
    }

    protected virtual void OnEnable()
    {
        GameServices.Get<UnitManager>()?.RegisterUnit(this);
    }

    protected virtual void OnDisable()
    {
        GameServices.Get<UnitManager>()?.UnregisterUnit(this);
    }

    public string UnitName => gameObject.name;
}
