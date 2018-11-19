using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Interface between the platform specific Flutter handlers and the native <see cref="IServiceHandler"/> implementation.
    /// </summary>
    public class MessageHandler
    {
        /// <summary>
        /// The service handler which will process all requests
        /// </summary>
        private readonly IServiceHandler _serviceHandler;

        public MessageHandler(IServiceHandler serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

        /// <summary>
        /// Handle a given method name and json encoded message.
        /// An appropriate method on the IServiceHandler is looked for which
        /// matches with the specified method name.
        /// From this the type of the message parameter is determined.
        /// The message is decoded from JSON, the method invoked and
        /// the value seen returned back.
        /// </summary>
        /// <returns>MessageResult instance</returns>
        /// <param name="method">Method.</param>
        /// <param name="messageJson">Message json.</param>
        /// 
        public Task<MessagingResult> Execute(string method, string messageJson)
        {
            // need to look for a method which is of the appropriate signature
            // i.e. 'called' method
            // from that we get the type of the message
            var methodInfo = _serviceHandler.GetType().GetMethods().FirstOrDefault(m => string.Compare(m.Name, method, StringComparison.OrdinalIgnoreCase) == 0);

            // ensure we have a match against the method name
            if (methodInfo == null)
            {
                throw new MissingMemberException($"{method}");
            }

            // check that we have a details parameter
            var parameters = methodInfo.GetParameters();

            if (parameters.Length != 1)
            {
                throw new MissingMethodException($"{method} - incorrect parameter count, should be 1, received {parameters.Length}");
            }

            var parameter = parameters[0];

            var parameterType = parameter.ParameterType;

            var messageObject = JsonConvert.DeserializeObject(messageJson, parameterType);

            var resultTask = methodInfo.Invoke(_serviceHandler, new object[] { messageObject });

            var task = (Task<MessagingResult>)resultTask;

            return task.ContinueWith(t => t.Result);
        }

    }
}
