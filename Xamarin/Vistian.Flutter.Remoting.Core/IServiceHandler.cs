using System;
namespace Vistian.Flutter.Remoting
{
    public interface IServiceHandler
    {
        void SetEventStreamFactory(IEventStreamFactory factory);
    }
}
