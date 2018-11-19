using System;
using Newtonsoft.Json;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// The result of processing a message.
    /// </summary>
    public class MessagingResult
    {
        public string JsonResult { get; private set; }

        public MessagingResult(object result)
        {
            JsonResult = JsonConvert.SerializeObject(result);
        }

        public static MessagingResult From(object result)
        {
            return new MessagingResult(result);
        }
    }

}
