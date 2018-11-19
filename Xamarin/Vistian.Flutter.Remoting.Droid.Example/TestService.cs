using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Vistian.Flutter.Remoting.Droid.Example
{
    public class TestService:ITestService
    {
        private readonly ServiceSettings _settings;

        public TestService(ServiceSettings settings)
        {
            _settings = settings;
        }
        public int CalcDouble(int original)
        {
            return 2 * original;
        }

        public Location GetLocationFor(string name)
        {
            return new Location() { Latitude = 53.15, Longitude = -2.0 };
        }

        public IObservable<Location> LocationTick(int amount)
        {
            return Observable.Interval(TimeSpan.FromSeconds(amount)).Select(a => new Location() { Latitude = a, Longitude = -a });
        }
    }
}