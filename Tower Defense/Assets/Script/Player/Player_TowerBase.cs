using System.Collections.Generic;
using UnityEngine;

public class Player_TowerBase : MonoBehaviour, IUnitInterface
{
    [Header("Unit Setting")]
    [SerializeField]
    protected float rotationSpeed = 5f;
    [SerializeField]
    protected float attackRange = 2.5f;
    [SerializeField]
    protected LayerMask enemyLayerMask;
    [SerializeField]
    protected float attackCooldown = 1;
    [SerializeField]
    protected Transform towerHead;
    [SerializeField]
    protected EnemyType primaryTargetType = EnemyType.None;
    [SerializeField]
    private bool dynamicTargetChange;

    protected float lastAttackTime;
    protected Enemy_Base currentEnemy = null;
    private readonly Collider[] enemiesToAttack = new Collider[10];
    private bool canRotate = true;
    private readonly List<Enemy_Base> priorityTargets = new List<Enemy_Base>();
    private readonly List<Enemy_Base> possibleTargets = new List<Enemy_Base>();
    private float targetCheckInterval = 0.1f;
    private float lastTimeCheckedTarget;

    protected virtual void Awake()
    { 

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

    protected virtual void Update()
    {
        UpdateTargetInAttacking();

        if (currentEnemy == null)
        {
            currentEnemy = FindEnemyWithinRange();
            return;
        }

        if (currentEnemy != null)
        {
            float enemyDistance = (currentEnemy.GetCenterPoint() - transform.position).sqrMagnitude;

            if (enemyDistance > attackRange * attackRange)
            {
                currentEnemy = null;
            }
        }
    }

    protected virtual void LateUpdate()
    {
        RotateTowerHeadToEnemy();

        if (CanAttack())
        {
            Attack();
        }
    }

    protected virtual Enemy_Base FindEnemyWithinRange()
    {
        priorityTargets.Clear();
        possibleTargets.Clear();

        int count = Physics.OverlapSphereNonAlloc(transform.position, attackRange, enemiesToAttack, enemyLayerMask);
        int loopCount = Mathf.Min(count, enemiesToAttack.Length);

        if (loopCount == 0)
        {
            return null;
        }

        for (int i = 0; i < loopCount; i++)
        {
            Enemy_Base newEnemy = enemiesToAttack[i].GetComponent<Enemy_Base>();
            if (newEnemy == null)
            {
                continue;
            }

            EnemyType newEnemyType = newEnemy.GetEnemyType();

            if (newEnemyType == primaryTargetType)
            {
                priorityTargets.Add(newEnemy);
            }
            else
            {
                possibleTargets.Add(newEnemy);
            }
        }

        if (priorityTargets.Count > 0)
        {
            return FindAdvancedEnemy(priorityTargets);
        }

        if (possibleTargets.Count > 0)
        {
            return FindAdvancedEnemy(possibleTargets);
        }

        return null;
    }

    protected virtual Enemy_Base FindAdvancedEnemy(List<Enemy_Base> enemies)
    {
        Enemy_Base mostAdvancedEnemy = null;
        float minRemainingDistance = float.MaxValue;

        foreach (Enemy_Base enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            float remainingDistance = enemy.GetDistanceToEndPoint();

            if (remainingDistance < minRemainingDistance)
            {
                minRemainingDistance = remainingDistance;
                mostAdvancedEnemy = enemy;
            }
        }

        return mostAdvancedEnemy;
    }

    protected bool CanAttack()
    {
        if (currentEnemy && Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            return true;
        }

        return false;
    }

    protected virtual void Attack()
    {

    }

    protected virtual void RotateTowerHeadToEnemy()
    {
        if (currentEnemy == null || !canRotate)
        {
            return;
        }

        Vector3 direction = GetDirectionToEnemy(towerHead);

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        towerHead.rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    protected Vector3 GetDirectionToEnemy(Transform startPoint)
    {
        if (currentEnemy == null)
        {
            return Vector3.zero;
        }

        return (currentEnemy.GetCenterPoint() - startPoint.position).normalized;
    }

    public void EnableRotation(bool isRotationEnable)
    {
        canRotate = isRotationEnable;
    }

    private void UpdateTargetInAttacking()
    {
        if (!dynamicTargetChange)
        {
            return;
        }

        if (Time.time > lastTimeCheckedTarget + targetCheckInterval)
        { 
            lastTimeCheckedTarget = Time.time;
            currentEnemy = FindEnemyWithinRange();
        }
    }

    public float GetAttackRange() => attackRange;

    public string UnitName => gameObject.name;
}
