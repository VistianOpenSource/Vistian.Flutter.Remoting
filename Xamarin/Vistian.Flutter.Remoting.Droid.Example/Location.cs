using System;
namespace Vistian.Flutter.Remoting.Droid.Example
{
    [FlutterExport()]
    public class Location
    {
        public double Latitude { get; set; } 

        public double Longitude { get; set; }
    }
}
