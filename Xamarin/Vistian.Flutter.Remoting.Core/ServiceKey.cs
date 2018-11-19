using System;
using System.Threading;
using Newtonsoft.Json;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Represents a unique identifier to a service
    /// </summary>
    public class ServiceKey
    {
        public string Value { get; }

        [JsonConstructor()]
        private ServiceKey(string value)
        {
            Value = value;
        }

        private static long _counter = 0;


        public static ServiceKey Create(string stub)
        {
            var unique = Interlocked.Increment(ref _counter);

            var key = $"{stub}#{unique}";

            return new ServiceKey(key);
        }

    }
}
