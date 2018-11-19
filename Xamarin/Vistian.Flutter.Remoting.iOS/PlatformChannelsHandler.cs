using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using Newtonsoft.Json;
using UIKit;

namespace Vistian.Flutter.Remoting
{
    /* Pending package 
    public class PlatformChannelsHandler : BasePlatformChannelsHandler<UIViewController>
    {
        protected static NSString detailsKey = new NSString(DetailsArgumentName);

        private readonly Dictionary<string, CallHandler> _methodCallHandlers = new Dictionary<string, CallHandler>();

        public PlatformChannelsHandler()
        {
        }

        public PlatformChannelsHandler(IEnumerable<KeyValuePair<string, IServiceHandler>> handlers) : base(handlers)
        {
        }

        public override void Start(UIViewController messenger)
        {
            foreach (var serviceHandler in ServiceHandlers)
            {
                // Create the channel
                var methodChannel = FlutterMethodChannel.MethodChannelWithName(serviceHandler.Key, messenger);

                // setup a message handler
                var messageHandler = new MessageHandler(serviceHandler.Value);

                // create our local callback handler invoked from Flutter
                var callHandler = new CallHandler(messageHandler);
                FlutterMethodCallHandler fmch = callHandler.FlutterMethodCallHandler;

                methodChannel.SetMethodCallHandler(fmch);

                // record the handler, this isn't used anywhere
                _methodCallHandlers[serviceHandler.Key] = callHandler;

                if (serviceHandler.Value.RaisesEvents)
                {
                    var flutterEventChannel = FlutterEventChannel.EventChannelWithName(GetEventChannelName(serviceHandler.Key), messenger);

                    var eventStream = new FlutterEventStream(flutterEventChannel);

                    serviceHandler.Value.SetEventStream(eventStream);
                }
            }
        }

        /// <summary>
        /// Internal class used to handle callbacks
        /// </summary>
        private class CallHandler
        {
            private MessageHandler _handler;

            public CallHandler(MessageHandler handler)
            {
                _handler = handler;
            }

            // callback from Flutter with a method...
            public async void FlutterMethodCallHandler(FlutterMethodCall methodCall, FlutterResult result)
            {
                try
                {
                    // decode the method details from the method call
                    var ns = (NSString)methodCall.Arguments.ValueForKey(detailsKey);

                    // invoke the registered handler for this method and the message details
                    var messagingResult = await _handler.Execute(methodCall.Method, ns.ToString());

                    // serialize up the message result
                    var jsonMessagingResult = JsonConvert.SerializeObject(messagingResult);

                    // and send it back
                    result(NSObject.FromObject(jsonMessagingResult));
                }
                catch (Exception exception)
                {
                    // report the error back to flutter
                    FlutterError.ErrorWithCode(typeof(Exception).Name, exception.Message, NSObject.FromObject(exception.Source));
                }
            }
        }
    }
    */
}