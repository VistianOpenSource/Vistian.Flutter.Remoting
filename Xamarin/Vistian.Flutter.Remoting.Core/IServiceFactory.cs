using System;
using System.Collections.Generic;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Remoting service factory.
    /// </summary>
    public interface IServiceFactory
    {
        object Resolve(string serviceName, List<Parameter> parameters);
    }
}
