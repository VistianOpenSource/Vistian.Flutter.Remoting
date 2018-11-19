using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Vistian.Flutter.Remoting
{
    public interface IClassServiceHandler : IServiceHandler
    {
        Task<MessagingResult> Create(CreateServiceMessage message);
        Task<MessagingResult> Invoke(InvokeMessage message);
        Task<MessagingResult> Dispose(DisposeMessage message);
    }

    public class ClassServiceHandler : IClassServiceHandler
    {
        private ServiceRepository _serviceRepository;
        private IServiceFactory _serviceFactory;

        public ClassServiceHandler(ServiceRepository serviceRepository,IServiceFactory serviceFactory)
        {
            _serviceRepository = serviceRepository;
            _serviceFactory = serviceFactory;
        }


        public Task<MessagingResult> Create(CreateServiceMessage message)
        {
            var service = _serviceFactory.Resolve(message.ServiceName, message.Parameters);

            ClassServiceInstance instance = new ClassServiceInstance(service);

            _serviceRepository.Add(instance);

            return Task.FromResult<MessagingResult>(new MessagingResult(instance.Key));
        }

        public Task<MessagingResult> Dispose(DisposeMessage message)
        {
            var key = message.Service;

            _serviceRepository.Dispose(key);

            return Task.FromResult<MessagingResult>(new MessagingResult(true));
        }

        public Task<MessagingResult> Invoke(InvokeMessage message)
        {
            Stopwatch sw = new Stopwatch();
            var key = message.Service;

            sw.Start();

            var service = _serviceRepository[key] as ClassServiceInstance;
            var t1 = sw.ElapsedTicks;
            InvocationTarget target = InvocationTarget.Create(service.Instance, message.Method,message.Parameters);
            var t2 = sw.ElapsedTicks;
            var result = target.Invoke().ContinueWith(r => new MessagingResult(r.Result));
            var t3 = sw.ElapsedTicks;

            sw.Stop();

            Console.WriteLine($"Invoke Method : {t1} {t2} {t3}");
            //return new MessagingResult(result);

            return result;

        }

        public void SetEventStreamFactory(IEventStreamFactory factory)
        {
            // do nothing
        }
    }
}
