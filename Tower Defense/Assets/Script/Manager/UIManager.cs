using System.Collections.Generic;
using System;

public class UIManager
{
    private static Dictionary<Type, Dictionary<string, object>> uiRegistry = new Dictionary<Type, Dictionary<string, object>>();

    public static void RegisterUI<T>(string uiName, T uiToRegister) where T : class
    {
        Type type = typeof(T);

        if (!uiRegistry.ContainsKey(type))
        {
            uiRegistry[type] = new Dictionary<string, object>();
        }

        if (!uiRegistry[type].ContainsKey(uiName))
        {
            uiRegistry[type].Add(uiName, uiToRegister);
        }
    }

    public static void UnregisterUI<T>(string uiName) where T : class
    {
        Type type = typeof(T);

        if (uiRegistry.ContainsKey(type))
        {
            uiRegistry[type].Remove(uiName);

            if (uiRegistry[type].Count == 0)
            {
                uiRegistry.Remove(type);
            }
        }
    }

    public static T GetUI<T>(string uiName) where T : class
    {
        Type type = typeof(T);

        if (uiRegistry.TryGetValue(type, out var subDict))
        {
            if (subDict.TryGetValue(uiName, out var uiInstance))
            {
                return uiInstance as T;
            }
        }

        return null;
    }
}
