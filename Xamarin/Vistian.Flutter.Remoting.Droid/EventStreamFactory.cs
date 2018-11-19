using System;
using IO.Flutter.Plugin.Common;
using IO.Flutter.View;

namespace Vistian.Flutter.Remoting
{
    public class EventStreamFactory : IEventStreamFactory
    {
        private FlutterView _messenger;
        private string _serviceHandlerName;
        private EventChannel _flutterEventChannel;
        private FlutterEventStream _eventStream;

        public EventStreamFactory(FlutterView messenger,string serviceHandlerName)
        {
            _messenger = messenger;
            _serviceHandlerName = serviceHandlerName;
        }

        public IEventStream CreateFor(ServiceKey service)
        {
            //if (_flutterEventChannel == null)

                Android.Util.Log.WriteLine(Android.Util.LogPriority.Info, "Vistian.Flutter.Remoting.Droid.Example", $"Creating Event Channel - {GetEventChannelName(service)}");
                var flutterEventChannel = new EventChannel(_messenger, GetEventChannelName(service));
                var eventStream = new FlutterEventStream(flutterEventChannel);

                Android.Util.Log.WriteLine(Android.Util.LogPriority.Info, "Vistian.Flutter.Remoting.Droid.Example", $"Complete Event Channel - {GetEventChannelName(service)}");

            return eventStream;
        }

        private string GetEventChannelName(ServiceKey service)
        {
            return $"{service.Value}/events";
        }
    }
}
