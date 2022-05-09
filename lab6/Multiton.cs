using System;
using System.Collections.Generic;
using System.Reflection;

public class Multiton<T> where T : class
{
    private static HashSet<T> instances = new HashSet<T>();

    private static T CreateInstance()
    {
        if (typeof(T).GetConstructors().Length > 0) {
            throw new Exception();
        }
        ConstructorInfo cInfo = typeof(T).GetConstructor (
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[0],
            new ParameterModifier[0]
        );
        return (T)cInfo.Invoke(null);
    }

    public static int Limit { get; set; }

    public static T Instance {
        get {
            if (instances.Count < Limit) {
                T newInstance = CreateInstance();
                instances.Add(newInstance);
                return newInstance;
            }
            else {
                return null;
            }
        }
    }

    public static void Delete (T obj)
    {
        instances.Remove(obj);
    }
}