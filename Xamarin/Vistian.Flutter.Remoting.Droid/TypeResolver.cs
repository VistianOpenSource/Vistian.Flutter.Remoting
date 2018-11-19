using System;
using System.Collections.Generic;
using System.Reflection;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Resolve a type given its name.
    /// </summary>
    public class TypeResolver:ITypeResolver
    {
        /// <summary>
        /// Cache of previously looked up types, case insensitive storage/lookup
        /// </summary>
        private readonly Dictionary<string, Type> _cache = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets all of the assemblies for this Android application.
        /// </summary>
        /// <returns>The assemblies.</returns>
        private static IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies;
        }

        /// <summary>
        /// Resolve the type for the named type.
        /// </summary>
        /// <returns>The resolve.</returns>
        /// <param name="typeName">Type name.</param>
        public Type Resolve(string typeName)
        {
            // check the cache first
            if (_cache.TryGetValue(typeName, out var matchedType))
            {
                return matchedType;
            }

            // get the list of assemblies
            var assemblies = GetAssemblies();

            // for each assembly see if the type is in there
            foreach(var assembly in assemblies)
            {
                var type = assembly.GetType(typeName,false,true);

                // if the type exists, cache it and we are done
                if (type != null)
                {
                    _cache[typeName] = type;
                    return type;
                }
            }

            // no match, return null.
            return null;
        }
    }
}