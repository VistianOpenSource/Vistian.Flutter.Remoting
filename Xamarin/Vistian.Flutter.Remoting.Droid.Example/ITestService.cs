using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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
    [FlutterExport()]
    public interface ITestService
    {

        int CalcDouble(int original);

        Location GetLocationFor(string name);

        IObservable<Location> LocationTick(int amount);
    }
}