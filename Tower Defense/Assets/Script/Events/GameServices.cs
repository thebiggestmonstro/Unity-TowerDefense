
using System;
using System.Collections.Generic;

public static class GameServices
{
    private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void Register<T>(T instance) where T : class => services[typeof(T)] = instance;

    public static void Unregister<T>(T instance) where T : class
    {
        if (services.TryGetValue(typeof(T), out object current) && ReferenceEquals(current, instance))
        {
            services.Remove(typeof(T));
        }
    }

    public static T Get<T>() where T : class => services.TryGetValue(typeof(T), out object instance) ? (T)instance : null;
}
