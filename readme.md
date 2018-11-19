# Xamarin & Flutter Remoting

## Introduction

An experimental release allowing for easier consumption of Xamarin based solutions from an embedded Flutter application.

## Motivation

Flutters design doesn't provide direct access to code executing within the host application. For many applications this may not be an issue but for those which need access to native capabilities of the host platform and/or have code which just can't be easily implemented in Flutter/Dart it can be quite time consuming to setup the calls between Flutter and the host. Even when this has been done you are then faced with constructing native code in potentially two different environments - XCode and Android Studio / Visual Code.

This experimental release includes code to not only make this interfacing easier but also a code generator to automatically produce Dart code to be used for this interfacing.

## Prerequisites

* Visual Studio 2017 for Windows.
* Vistian Flutter Bindings for Flutter 0.9.4.

## Caveats

* This is a work in progress, I'm sure there will be places where it fails,leaks memory etc. 

* This is currently for Android only.

* This has been tested on a Nexus 6P device.

* This is not designed to allow for the delivery of release solutions

## Usage

Essentially there two elements to making this work:

1. Installing the T4 script in your .Net project automatically generate Dart source
2. Installing the Dart package which provides the support classes for the Denerated dart source.

### Xamarin

1. Install the Remoting.tt file within your project(s) for which code generation is required.
2. Mark those interfaces which you wish to make available through Dart with a FlutterExport attribute. e.g.


```
    [FlutterExport()]
    public interface ITestService
    {
        int CalcDouble(int original);

       Location GetLocationFor(string name);

       IObservable<Location> LocationTick(int amount);
    }
```

3. Save the template or right hand mouse click and choose 'Run Custom Tool'. 

The template will then scan all of the code within the project looking for interfaces and POCO items for converting to Flutter/Dart. If it finds referenced classes along the way in the processing of interfaces there are added to the list to have Dart code generated for them as well (assumption here being POCO classes are referenced). The original C# code comments should also be automatically brought across.

Currently the code looks for methods which it can translate into calls and for POCO objects for which it looks at their properties. Properties on the interfaces could also be easily added along with classes, the majority of the tools exists within the T4 template to make this a relatively simple task.

The result of running the template results in a file looking  somelike like :

```
// Generated Code - 16/11/2018 16:42:07

import 'dart:async';
import 'package:json_annotation/json_annotation.dart';
import 'package:rxdart/rxdart.dart';
import 'package:vistian_remoting/remoting.dart';

part 'remoteServices.g.dart';
//
class $_ITestServiceMixin extends Remoting
{
  static Service _service = Service("Vistian.Flutter.Remoting.Droid.Example.ITestService");
  static _RemoteTypeMap _typeMap = _RemoteTypeMap();

  Service get service => _service;
  RemoteTypeMapper get remoteTypeMapper => _typeMap; 

  //
  Future<int> calcDouble(int original) async { 
  Map<String,dynamic> params = new Map();
  params["original"] = original;

  return await $_invoke<int>("CalcDouble",params:params,converter:(r) => (r as num)?.toInt());		
  }
	    
  //
  Future<Location> getLocationFor(String name) async { 
  Map<String,dynamic> params = new Map();
  params["name"] = name;

  return await $_invoke<Location>("GetLocationFor",params:params,converter:(r) => Location.fromJson(r));		
  }
	    
  //
  Future<Observable<Location>> locationTick(int amount) async { 
  Map<String,dynamic> params = new Map();
  params["amount"] = amount;

  return await $_invokeObservable<Location>("LocationTick",params:params,converter:(r) => Location.fromJson(r));		
  }
	    }
//
@JsonSerializable()
class Location extends Object {				
// 
  @JsonKey(name:'Latitude')
  double latitude;
// 
  @JsonKey(name:'Longitude')
  double longitude;
  Location(this.latitude,this.longitude);

  factory Location.fromJson(Map<String,dynamic> json) => _$LocationFromJson(json);

  Map<String,dynamic> toJson() => _$LocationToJson(this);
}
//
@JsonSerializable()
class ServiceSettings extends Object {				
// 
  @JsonKey(name:'Tolerance')
  int tolerance;
// 
  @JsonKey(name:'Period')
  int period;
  ServiceSettings(this.tolerance,this.period);

  factory ServiceSettings.fromJson(Map<String,dynamic> json) => _$ServiceSettingsFromJson(json);

  Map<String,dynamic> toJson() => _$ServiceSettingsToJson(this);
}
class _RemoteTypeMap extends RemoteTypeMapper {
  static Map<String,String> dartToRemote = <String,String> {
    "int" : "System.Int32",
    "String" : "System.String",
    "Location" : "Vistian.Flutter.Remoting.Droid.Example.Location",
    "ServiceSettings" : "Vistian.Flutter.Remoting.Droid.Example.ServiceSettings",
    "double" : "System.Double",
  };

  _RemoteTypeMap():super(dartToRemote);
}

```

As well as most of the standard C# types being supported, Lists,Dictionaries,Tasks and Observables are also supported. 

The adddition of new constructs e.g. arrays can be easily added, just look at the source code.

4. Setup your environment for handing of these calls. Much of these is done by reflection, so the amount of code needed is quite small. The 'resolution' of services into actual classes can be either done by hand, or an example Autofac provider has been constructed. A typical setup would look like this
```
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

```

### Flutter

1. Add a reference to the support package vistian_remoting. This **hasn't** been uploaded to the main pub server since there seems to be no way to have test releases and I wasn't comfortable with publishing something that is clearly not of release quality.
2. Copy in/link to the generated source file(s). 
3. Ensure you have json_annotation, rxdart, build_runner and json_serializable dependencies setup correctly in your project.
4. Use within your flutter solution as you would with the Vistian Flutter hosting package.
5. Depending upon whether you have setup your T4 template to generate mixin code or classes, you may have to create 'concrete' implementations of your service classes e.g.


```
class TestService extends $_ITestServiceMixin {

  // mechanism used to provide 'constructor' details for a remote service
  @override
  Map<String,dynamic > initializationParameters() {
    return <String,dynamic>{'settings' : new ServiceSettings(100,200)};
  }
}
```

It should be noted that it is possible to provide constructor parameters through the user of the initializationParameters overload. In the case of Autofac these would be converted into named parameters and the service constructed with them.


## Final Thoughts

1. Support for classes along with interface properties may well make sense in being implemented.
2. JSON is currently used as the transport mechanism, this could possibly be changed to a more efficient form.
3. Error handling is 'rough' (both for generation and runtime) and could be a lot better.
4. Roslyn make be a better option for the code generator.
