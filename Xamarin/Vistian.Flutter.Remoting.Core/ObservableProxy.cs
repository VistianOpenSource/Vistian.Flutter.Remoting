using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Base Observable proxy used to observable a local Observable and transfer observations to the <see cref="IEventStream"/>
    /// </summary>
    public abstract class ObservableProxy
    {
        public IEventStream EventStream { get; protected set; }

        public ServiceKey Key { get; protected set; }

        public abstract void OnListen();

        public abstract void OnDispose();
    }

    public class ObservableProxy<T>:ObservableProxy
    {
        // so then, we have the actual proxy to the observable...
        readonly IObservable<T> _observable;

        IDisposable _subscription;

        public ObservableProxy(IObservable<T> observable,IEventStream eventStream,ServiceKey key)
        {
            _observable = observable;
            EventStream = eventStream;
            Key = key;
        }

        public ObservableProxy(IObservable<T> observable)
        {
            _observable = observable;
        }

        public override void OnListen()
        {
            _subscription = _observable.Materialize().Subscribe(OnMaterialized);
        }

        private void OnMaterialized(Notification<T> m)
        {
            var notification = ObservableNotificationMessage.FromNotification(Key,m);
            EventStream.Publish(notification);
        }

        public override void OnDispose()
        {
            _subscription?.Dispose();

            _subscription = null;
        }
    }
}
