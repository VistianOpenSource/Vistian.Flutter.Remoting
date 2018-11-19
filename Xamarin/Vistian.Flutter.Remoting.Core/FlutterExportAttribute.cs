using System;

namespace Vistian.Flutter.Remoting
{

    /// <inheritdoc />
    /// <summary>
    /// Attribute used to mark an interface or a POCO class for Dart remoting code generation.
    /// </summary>
    public sealed class FlutterExportAttribute : Attribute
    {
        public readonly bool Export;
        public FlutterExportAttribute(bool export = true)
        {
            Export = export;
        }
    }
}
