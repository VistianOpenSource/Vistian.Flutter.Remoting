using System;
using System.Collections.Generic;

namespace Vistian.Flutter.Remoting
{
    /// <summary>
    /// A request to create an observable service.
    /// </summary>
    public class CreateServiceObservableMessage
    {
        /// <summary>
        /// Get or set the existing service associated with the 
        /// </summary>
        /// <value>The service.</value>
        public ServiceKey Service { get; set; }

        /// <summary>
        /// Gets or sets the method name
        /// </summary>
        /// <value>The method.</value>
        public String Method { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The arguments.</value>
        public List<Parameter> Parameters { get; set; }
    }

}
