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
using IO.Flutter.View;
using Newtonsoft.Json;

namespace Vistian.Flutter.Remoting.Droid
{
    public class PlatformChannelsHandler : BasePlatformChannelsHandler<FlutterView>
    {      
        private readonly Dictionary<string, CallHandler> _methodCallHandlers = new Dictionary<string, CallHandler>();

        private Dictionary<string, IEventStream> _factories = new Dictionary<string, IEventStream>();
         public PlatformChannelsHandler()
        {
        }

        public PlatformChannelsHandler(IEnumerable<KeyValuePair<string, IServiceHandler>> handlers) : base(handlers)
        {
        }

        public override void Start(FlutterView messenger)
        {

            foreach (var serviceHandler in ServiceHandlers)
            {
                
                // Create the channel
                var methodChannel = new MethodChannel(messenger,serviceHandler.Key);

                // setup a message handler
                var messageHandler = new MessageHandler(serviceHandler.Value);

                // create our local callback handler invoked from Flutter
                var callHandler = new CallHandler(messageHandler);

                methodChannel.SetMethodCallHandler(callHandler);

                // record the handler, this isn't used anywhere
                _methodCallHandlers[serviceHandler.Key] = callHandler;

                var eventStreamFactory = new EventStreamFactory(messenger, serviceHandler.Key);
                //_factories[serviceHandler.Key] = eventStreamFactory.CreateFor(ServiceKey.Create("bongo")); 

                serviceHandler.Value.SetEventStreamFactory(eventStreamFactory);
            }
        }

        /// <summary>
        /// Internal class used to handle callbacks
        /// </summary>
        private class CallHandler:Java.Lang.Object,MethodChannel.IMethodCallHandler
        {
            private readonly MessageHandler _handler;

            public CallHandler(MessageHandler handler)
            {
                _handler = handler;
            }

            // callback from Flutter with a method...
            public async void OnMethodCall(MethodCall methodCall, MethodChannel.IResult result)
            {
                try
                {
                    Android.Util.Log.WriteLine(Android.Util.LogPriority.Info, "Vistian.Flutter.Remoting.Droid.Example", $"Message received {methodCall.Method} for {_handler.GetType().Name}");
                    Stopwatch stopWatch = new Stopwatch();

                    stopWatch.Start();

                    // decode the method details from the method call
                    var arg = methodCall.Argument(DetailsArgumentName);


                    var t0 = stopWatch.ElapsedTicks;
                    var ns = arg.ToString();


                    var t1 = stopWatch.ElapsedTicks;
                    // invoke the registered handler for this method and the message details
                    var messagingResult = await _handler.Execute(methodCall.Method,ns).ConfigureAwait(false);
                    var t2 = stopWatch.ElapsedTicks;
                    // serialize up the message result, 
                    var jsonMessagingResult = JsonConvert.SerializeObject(messagingResult);
                    var t3 = stopWatch.ElapsedTicks;
                    stopWatch.Stop();

                    Android.Util.Log.WriteLine(Android.Util.LogPriority.Info,"Vistian.Flutter.Remoting.Droid.Example" ,$"Messaging Handling took {stopWatch.ElapsedMilliseconds} {t0} {t1} {t2} {t3} freq: {Stopwatch.Frequency}");
                    // and send it back
                    result.Success(jsonMessagingResult);
                }
                catch (Exception exception)
                {
                    // report the error back to flutter
                    result.Error(exception.GetType().Name, exception.Message, exception.Source);
                }

            }
        }
    }
}