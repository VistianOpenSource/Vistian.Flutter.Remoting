using System;
using System.Collections.Generic;
using System.Linq;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Represents a single service instance.
    /// </summary>
    public abstract class ServiceInstance : IDisposable
    {
        public ServiceKey Key { get; }

        // in effect the parent
        public ServiceInstance Container { get; }

        protected ServiceInstance(ServiceInstance container, ServiceKey key)
        {
            Container = container;
            Key = key;
        }

        /// <summary>
        /// The children that were created by this service
        /// </summary>
        private List<ServiceInstance> _children = new List<ServiceInstance>();

        public void AddChild(ServiceInstance child)
        {
            _children.Add(child);
        }

        public void RemoveChild(ServiceInstance child)
        {
            var matchingChild = _children.FirstOrDefault(c => c.Key == child.Key);

            if (matchingChild != null)
            {
                _children.Remove(matchingChild);
            }
        }


        public virtual void Dispose()
        {
            if (_children == null) return;


            foreach (var child in _children)
            {
                child.Dispose();
            }
            _children.Clear();
            _children = null;
        }
    }
}
