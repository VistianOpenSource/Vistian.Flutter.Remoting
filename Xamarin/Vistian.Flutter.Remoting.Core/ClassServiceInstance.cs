using System;
namespace Vistian.Flutter.Remoting
{
    public class ClassServiceInstance : ServiceInstance
    {
        public object Instance { get; private set; }

        public override void Dispose()
        {
            base.Dispose();
            (Instance as IDisposable)?.Dispose();
            Instance = null;
        }

        public ClassServiceInstance(object instance) : base(null, ServiceKey.Create(instance.GetType().FullName))
        {
            Instance = instance;
        }
    }
}
