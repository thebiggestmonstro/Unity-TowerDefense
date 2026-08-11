using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private readonly Dictionary<System.Type, HashSet<object>> registeredUnits = new Dictionary<System.Type, HashSet<object>>();

    public static UnitManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterUnit<T>(T unit) where T : class, IUnitInterface
    {
        var type = typeof(T);
        if (!registeredUnits.ContainsKey(type))
        {
            registeredUnits[type] = new HashSet<object>();
        }

        registeredUnits[type].Add(unit);
    }

    public void UnregisterUnit<T>(T entity) where T : class, IUnitInterface
    {
        var type = typeof(T);
        if (registeredUnits.ContainsKey(type))
        {
            registeredUnits[type].Remove(entity);
        }
    }

    public bool FindContainsUnit<T>(T entity) where T : class, IUnitInterface
    {
        var type = typeof(T);
        return registeredUnits.ContainsKey(type) && registeredUnits[type].Contains(entity);
    }

    public T GetUnitByName<T>(string unitName) where T : class, IUnitInterface
    {
        var type = typeof(T);

        if (registeredUnits.TryGetValue(type, out var entities))
        {
            foreach (var entity in entities)
            {
                if (entity is T typedEntity && typedEntity.UnitName == unitName)
                {
                    return typedEntity;
                }
            }
        }

        return null;
    }

    public List<T> GetUnits<T>() where T : class, IUnitInterface
    {
        var type = typeof(T);
        var resultList = new List<T>();

        if (registeredUnits.TryGetValue(type, out var entities))
        {
            foreach (var entity in entities)
            {
                if (entity is T typedEntity)
                {
                    resultList.Add(typedEntity);
                }
            }
        }

        return resultList;
    }
}
