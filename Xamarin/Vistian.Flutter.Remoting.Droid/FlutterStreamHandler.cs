using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IO.Flutter.Plugin.Common;
using Newtonsoft.Json;
using Object = Java.Lang.Object;

namespace Vistian.Flutter.Remoting
{
    public class FlutterStreamHandler :Java.Lang.Object,EventChannel.IStreamHandler
    {
        private EventChannel.IEventSink _eventSink;

        public void Publish(object item)
        {
            var json = JsonConvert.SerializeObject(item);

            _eventSink?.Success(json);
        }

        public void OnCancel(Object p0)
        {
            _eventSink = null;
        }

        public void OnListen(Object p0, EventChannel.IEventSink p1)
        {
            _eventSink = p1;
        }
    }
}