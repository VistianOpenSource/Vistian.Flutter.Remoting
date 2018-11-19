using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vistian.Flutter.Remoting
{
    public interface IPlatformChannelsHandler<T>
    {
        Dictionary<string, IServiceHandler> Handlers { get; set; }
        void Start(T param);
    }

    public class BasePlatformChannelsHandler<T>: IPlatformChannelsHandler<T>,IEnumerable<KeyValuePair<string, IServiceHandler>>
    {
        protected const string DetailsArgumentName = "details";

        protected Dictionary<string, IServiceHandler> ServiceHandlers = new Dictionary<string, IServiceHandler>();

        public Dictionary<string, IServiceHandler> Handlers { get => ServiceHandlers; set => ServiceHandlers = value; }

        public IServiceHandler this[string key]
        {
            get => ServiceHandlers[key];
            set => Add(key, value);
        }

        public void Add(string key, IServiceHandler handler)
        {
            ServiceHandlers[key] = handler;
        }

        public IEnumerator<KeyValuePair<string, IServiceHandler>> GetEnumerator()
        {
            return ServiceHandlers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ServiceHandlers.GetEnumerator();
        }

        public BasePlatformChannelsHandler()
        {
        }

        public BasePlatformChannelsHandler(IEnumerable<KeyValuePair<string, IServiceHandler>> handlers)
        {
            foreach (var handler in handlers)
            {
                this[handler.Key] = handler.Value;
            }
        }

        public static string GetEventChannelName(string serviceHandlerName)
        {
            return $"{serviceHandlerName}/events";
        }

        public virtual void Start(T param)
        {
        }
    }
}
