using System;
using System.Collections.Generic;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// The repository of all created service instances.
    /// </summary>
    public class ServiceRepository
    {
        private readonly Dictionary<ServiceKey, ServiceInstance> _services = new Dictionary<ServiceKey, ServiceInstance>(new ServiceKeyComparer());

        public void Add(ServiceInstance instance)
        {
            _services[instance.Key] = instance;
        }

        public void Dispose(ServiceKey key)
        {
            var service = _services[key];

            service.Dispose();

            _services.Remove(key);
        }

        public T Get<T>(ServiceKey key) where T:ServiceInstance
        {
            var instance = _services[key];

            return instance is T ? (T)instance : null;
        }

        public ServiceInstance this[ServiceKey key] => _services[key];
    }
}
