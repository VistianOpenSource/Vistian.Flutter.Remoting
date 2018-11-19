using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Newtonsoft.Json;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Autofac based service factory.
    /// </summary>
    public class AutofacServiceFactory : IServiceFactory
    {
        private readonly IContainer _container;
        private readonly ITypeResolver _typeResolver;

        /// <summary>
        /// Initialize with the autofac container
        /// </summary>
        /// <param name="container">Container.</param>
        /// <param name="typeResolver"></param>
        public AutofacServiceFactory(IContainer container,ITypeResolver typeResolver)
        {
            _container = container;
            _typeResolver = typeResolver;
        }

        /// <summary>
        /// Resolve the service given its name and the optional parameters
        /// </summary>
        /// <returns>The resolved service</returns>
        /// <param name="serviceName">Service name.</param>
        /// <param name="parameters">Parameters.</param>
        public object Resolve(string serviceName, List<Parameter> parameters)
        {
            // now the service type is the full type name
            var serviceType = _typeResolver.Resolve(serviceName);

            // Decode the parameters into the relevant types..
            var namedParameters = parameters?.Select(parameter =>
            {
                var resolvedType = _typeResolver.Resolve(parameter.ValueType);
                var value = JsonConvert.DeserializeObject(parameter.Value.ToString(),resolvedType);
                var np = new NamedParameter(parameter.Name, value);
                return np;
            });

            return _container.Resolve(serviceType,namedParameters);
        }
    }
}
