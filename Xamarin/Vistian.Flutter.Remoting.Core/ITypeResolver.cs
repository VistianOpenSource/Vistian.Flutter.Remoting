using System;
namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Resolve a type given the types name, which includes the namespasce.
    /// </summary>
    public interface ITypeResolver
    {
        Type Resolve(string typeName);
    }
}
