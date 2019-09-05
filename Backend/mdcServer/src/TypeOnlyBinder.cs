using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// removes assembly name from type resolution
/// </summary>
public class TypeOnlyBinder : SerializationBinder
{
    private static SerializationBinder defaultBinder = new BinaryFormatter().Binder;



    public override Type BindToType(string assemblyName, string typeName)
    {
        if (assemblyName.Equals("NA"))
            return Type.GetType(typeName);
        else
            return defaultBinder.BindToType(assemblyName, typeName);

    }

    public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        // but null out the assembly name
        assemblyName = "NA";
        typeName = serializedType.FullName;

    }

    private static object locker = new object();
    private static TypeOnlyBinder _default = null;

    public static TypeOnlyBinder Default
    {
        get
        {
            lock (locker)
            {
                if (_default == null)
                    _default = new TypeOnlyBinder();
            }
            return _default;
        }
    }
}