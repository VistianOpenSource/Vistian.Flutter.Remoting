using System;
using System.Collections.Generic;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// Represents the request to initialize a service
    /// </summary>
    public class CreateServiceMessage
    {
        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the optional parameters for the initialization of the service
        /// </summary>
        /// <value>The parameters.</value>
        public List<Parameter> Parameters { get; set; }
    }


}
