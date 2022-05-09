using System;
using System.Reflection;

public class Singleton<T> where T : class
{
    private static T instance;

    private static T CreateInstance()
    {
        if (typeof(T).GetConstructors().Length > 0) {
            throw new Exception
                ("Strict singleton does not allow public constructors.");
        }
        ConstructorInfo cInfo = typeof(T).GetConstructor (
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[0],
            new ParameterModifier[0]
        );
        return (T)cInfo.Invoke(null);
    }

    public static T Instance {
        get {
            if (instance == null) instance = CreateInstance();
            return instance;
        }
    }
}