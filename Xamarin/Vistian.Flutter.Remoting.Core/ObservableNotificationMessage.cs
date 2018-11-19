using System;
using System.Collections.Generic;
using System.Reactive;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Vistian.Flutter.Remoting
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotificationKind { 
        OnData , 
        OnError , 
        OnCompleted };

    public class ObservableNotificationMessage
    {
        private static Dictionary<System.Reactive.NotificationKind, NotificationKind> _map = 
            new Dictionary<System.Reactive.NotificationKind, NotificationKind>(){

        {System.Reactive.NotificationKind.OnNext,NotificationKind.OnData},
        {System.Reactive.NotificationKind.OnCompleted,NotificationKind.OnCompleted},
        {System.Reactive.NotificationKind.OnError,NotificationKind.OnError}

    };
        [JsonConverter(typeof(StringEnumConverter))]

        public NotificationKind Kind { get; private set; }

        public string Exception { get; private set; }

        public string Value { get; private set; }

        public ServiceKey Service { get; private set; }

        public ObservableNotificationMessage() { }

        public static ObservableNotificationMessage FromNotification<T>(ServiceKey service, Notification<T> notification)
        {
            var on = new ObservableNotificationMessage
            {
                Kind = _map[notification.Kind]
            };

            switch (@on.Kind)
            {
                case NotificationKind.OnData:
                    @on.Value = JsonConvert.SerializeObject(notification.Value);
                    break;
                case NotificationKind.OnError:
                    @on.Exception = notification.Exception.Message;
                    break;
            }

            on.Service = service;

            return on;
        }
    }
}
