using System;
namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Contains an Observable 
    /// </summary>
    public sealed class ObservableServiceInstance : ServiceInstance
    {
        public ObservableProxy ObservableProxy { get; private set; }

        public ObservableServiceInstance(ClassServiceInstance container,
                                         ObservableProxy observableProxy,
                                         string name) : this(container,observableProxy,CreateKeyFor(container,name))
        {
        }

        public ObservableServiceInstance(ClassServiceInstance container,ObservableProxy observableProxy,ServiceKey key):base(container,key)
        {
            ObservableProxy = observableProxy;
        }
        public override void Dispose()
        {
            ObservableProxy?.OnDispose();
            ObservableProxy = null;
        }


        public static ServiceKey CreateKeyFor(ServiceInstance container, string method)
        {
            return ServiceKey.Create($"{container.Key}_{method}");
        }

    }

}
