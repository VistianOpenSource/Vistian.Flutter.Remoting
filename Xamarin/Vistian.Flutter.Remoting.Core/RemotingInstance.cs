using System;
namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Represents the platform specific platform channels handler and the associated service repository
    /// </summary>
    public class RemotingInstance<T>
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public ServiceRepository Repository { get; private set; }

        /// <summary>
        /// Gets the platform channel handler.
        /// </summary>
        /// <value>The channel handler.</value>
        public IPlatformChannelsHandler<T> ChannelHandler { get; private set; }

        public RemotingInstance(ServiceRepository repository, IPlatformChannelsHandler<T> channelHandler)
        {
            Repository = repository;
            ChannelHandler = channelHandler;
        }

        /// <summary>
        /// Start the platform channel handler with the specific param.
        /// </summary>
        /// <param name="param">Parameter.</param>
        public void Start(T param)
        {
            ChannelHandler.Start(param);
        }

    }
}
