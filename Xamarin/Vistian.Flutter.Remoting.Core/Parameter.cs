using System;
namespace Vistian.Flutter.Remoting
{
    public class Parameter
    {
        /// <summary>
        /// Get or set the parameter name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }

        /// <summary>
        /// Get or set the Dart Value Type
        /// </summary>
        /// <value>The type of the value.</value>
        public string ValueType { get; set; }
    }
}
