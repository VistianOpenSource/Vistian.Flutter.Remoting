using System;
namespace Vistian.Flutter.Remoting
{
    public interface IEventStreamFactory
    {
        IEventStream CreateFor(ServiceKey service);
    }
}
