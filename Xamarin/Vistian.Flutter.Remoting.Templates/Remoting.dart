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

