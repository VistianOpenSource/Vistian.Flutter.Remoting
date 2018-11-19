using System;
using System.Collections.Generic;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Represents the invocation of a method for a specified instance .
    /// </summary>
    public class InvokeMessage
    {
        /// <summary>
        /// Gets or sets the key for the associated service.
        /// </summary>
        /// <value>The instance.</value>
        public ServiceKey Service { get; set; }

        /// <summary>
        /// Gets or sets the method name
        /// </summary>
        /// <value>The method.</value>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The arguments.</value>
        public List<Parameter> Parameters { get; set; }
    }
}
