using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Autofac;
using IO.Flutter.View;
using Vistian.Flutter.Remoting;
using Vistian.Flutter.Remoting.Droid;
using Vistian.Flutter.Remoting.Droid.Example;

namespace Sample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // create flutter view host
            var flutterView = Flutter.Bindings.Flutter.CreateView(this, this.Lifecycle, "home");
            var layout = new FrameLayout.LayoutParams(600, 800) { LeftMargin = 200, TopMargin = 400 };

            // setup Autofac 
            var cb = new ContainerBuilder();
            cb.RegisterType<TestService>().As<ITestService>();
            var container = cb.Build();

            // create the service factory
            var serviceFactory = new AutofacServiceFactory(container,new TypeResolver());
            // create the remoting instance
            var remotingInstance = Remoting.Create<FlutterView, PlatformChannelsHandler>(serviceFactory);
            // and tell Flutter that 'we are listening'
            remotingInstance.Start(flutterView);

            AddContentView(flutterView, layout);
        }
	}
}

