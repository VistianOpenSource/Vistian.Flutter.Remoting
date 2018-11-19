using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IO.Flutter.Plugin.Common;

namespace Vistian.Flutter.Remoting
{
    public class FlutterEventStream : IEventStream
    {
        private readonly EventChannel _channel;
        private readonly FlutterStreamHandler _streamHandler;

        public FlutterEventStream(EventChannel channel)
        {
            _channel = channel;

            _streamHandler = new FlutterStreamHandler();

            _channel.SetStreamHandler(_streamHandler);

        }

        public void Publish(object data)
        {
            _streamHandler.Publish(data);
        }
    }
}