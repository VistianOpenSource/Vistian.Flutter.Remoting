using System;
namespace Vistian.Flutter.Remoting
{
    public interface IEventStream
    {
        void Publish(object @event);
    }
}
