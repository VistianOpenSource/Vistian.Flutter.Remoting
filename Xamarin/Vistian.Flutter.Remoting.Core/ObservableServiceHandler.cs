using System;
using System.Linq;
using System.Threading.Tasks;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Provide handling for the provision of Observable services.
    /// </summary>
    public class ObservableServiceHandler:IServiceHandler
    {

        /// <summary>
        /// The master service repository
        /// </summary>
        private readonly ServiceRepository _serviceRepository;
        private IEventStreamFactory _factory;

        public ObservableServiceHandler(ServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        /// <summary>
        /// Create an observable service from the specified message.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="message">Message.</param>
        public async Task<MessagingResult> Create(CreateServiceObservableMessage message)
        {
            // lookup the existing service, it MUST exist
            var service = _serviceRepository[message.Service];

            if (service == null)
            {
                throw new InvalidOperationException($"No Service associated with {message.Service.Value}");
            }

            // get the target for the creation of the service and invoke it
            var target = InvocationTarget.Create((service as ClassServiceInstance).Instance, message.Method, message.Parameters);
            var result = await target.Invoke();

            if (!target.IsObservable)
            {
                throw new InvalidOperationException($"Service {message.Service.Value} Method {message.Method} doesn't return an Observable");
            }

            // now then, we need to spin up our observable proxy with our resultant observable (which we should have)
            var interfaces = result.GetType().GetInterfaces();

            var i = interfaces.FirstOrDefault(ii => ii.IsConstructedGenericType && typeof(IObservable<>) == ii.GetGenericTypeDefinition());

            var genericType = i.GetGenericArguments()[0];
            var genericOwType = typeof(ObservableProxy<>);
            var gen = genericOwType.MakeGenericType(genericType);

            var serviceKey = ObservableServiceInstance.CreateKeyFor(service, message.Method);
            var eventStream = _factory.CreateFor(serviceKey);
            var observableProxy = (ObservableProxy)Activator.CreateInstance(gen, new object[] { result,eventStream,serviceKey });

            // create the service instance, store it away 
            var si = new ObservableServiceInstance((ClassServiceInstance)service, observableProxy, serviceKey);
            _serviceRepository.Add(si);

            // and return a message with the key of the observable instance
            return new MessagingResult(si.Key);
        }

        public Task<MessagingResult> Subscribe(ObservableSubscribeMessage message)
        {
            if (!(_serviceRepository[message.Service] is ObservableServiceInstance service))
            {
                throw new InvalidOperationException($"No Observable Service associated with {message.Service.Value}");
            }

            service.ObservableProxy.OnListen();

            return Task.FromResult<MessagingResult>(new MessagingResult(true));
        }


        public Task<MessagingResult> Dispose(DisposeMessage message)
        {
            _serviceRepository.Dispose(message.Service);

            return Task.FromResult<MessagingResult>(new MessagingResult(true));
        }

        public void SetEventStreamFactory(IEventStreamFactory factory)
        {
            _factory = factory;
        }
    }
}
