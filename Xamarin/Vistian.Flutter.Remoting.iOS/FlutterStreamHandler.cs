using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Newtonsoft.Json;
using UIKit;

namespace Vistian.Flutter.Remoting
{
    /*
    public class FlutterStreamHandler : NSObject, IFlutterStreamHandler
    {
        private FlutterEventSink _events;

        public FlutterError OnCancelWithArguments(NSObject arguments)
        {
            return null;
        }

        public FlutterError OnListenWithArguments(NSObject arguments, FlutterEventSink events)
        {
            _events = events;

            //events(new(););
            return null;
            //throw new NotImplementedException();
        }

        public void Publish(object item)
        {
            var json = JsonConvert.SerializeObject(item);

            if (_events != null)
            {
                _events(NSObject.FromObject(json));
            }
        }
        
    }*/
}