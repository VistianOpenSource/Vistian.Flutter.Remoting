using System;
using System.Collections.Generic;

namespace Vistian.Flutter.Remoting
{
    public static class Remoting
    {
        // this needs to be all, pretty much platform agnostic... the platform channels handler however will be different

        public static RemotingInstance<T> Create<T,THandler>(IServiceFactory serviceFactory,Dictionary<string,IServiceHandler> handlers = null) where THandler : IPlatformChannelsHandler<T>,new()
        {
            // STANDARD
            var serviceRepository = new ServiceRepository();

            // STANDARD
            if (handlers == null)
            {
                handlers = GetStandardHandlers(serviceRepository, serviceFactory);
            }

            // PLATFORM SPECIFIC
            var platformHandler = new THandler
            {
                Handlers = handlers
            };

            // PLATFORM SPECIFIC
            return new RemotingInstance<T>(serviceRepository,platformHandler);
        }


        // Standard set
        private static Dictionary<string,IServiceHandler> GetStandardHandlers(ServiceRepository serviceRepository, IServiceFactory serviceFactory)
        {
            return new Dictionary<string,IServiceHandler>
            {
                ["remoting"] = new ClassServiceHandler(serviceRepository, serviceFactory),
                ["observables"] = new ObservableServiceHandler(serviceRepository)
            };
        }
    }
}
