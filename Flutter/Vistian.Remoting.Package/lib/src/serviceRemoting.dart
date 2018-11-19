/*
   Base class used for interfacing with remote services through a Flutter MethodChannel
 */
import 'dart:convert';
import 'package:flutter/services.dart';
import 'disposeMessage.dart';
import 'invokeMessage.dart';
import 'observableSubscribeMessage.dart';
import 'serviceKey.dart';
import 'createServiceMessage.dart';
import 'parameter.dart';
import 'service.dart';

import 'messagingResult.dart';

class RemotingService{
  final MethodChannel _platform;

  static const String parameterDetailsKey = "details";

  RemotingService(String methodChannel):this._platform=new MethodChannel(methodChannel);

  Future<dynamic> sendMessage(String method,dynamic message) async{
    Map<String,String> p = new Map<String,String>();
    p[parameterDetailsKey] = json.encode(message.toJson());

    var rawResult = await _platform.invokeMethod(method,p);

    MessagingResult messageResult = MessagingResult.fromJson(json.decode(rawResult));
    return json.decode(messageResult.jsonResult);
  }
}

/*
   Standard class for construction of a remote service along with invocation and disposal constructs.
*/
class Services extends RemotingService{
  static const String  _methodChannel = "remoting";
  static const String _createMethod = "create";
  static const String _disposeMethod = "dispose";
  static const String _invokeMethod = "invoke";

  static Services instance = new Services();

  Services():super(_methodChannel);

  /*
    Create a service instance
   */
  Future<ServiceKey> create(Service service,List<Parameter> parameters) async{
    var message = CreateServiceMessage(service.name, parameters);

    var jsonResult = await sendMessage(_createMethod, message);

    return ServiceKey.fromJson(jsonResult);
  }

  /*
     Invoke a specified method on an existing service instance.
   */
  Future<dynamic> invoke(ServiceKey service,String method,List<Parameter> parameters) async {
    var message = InvokeMessage(service, method, parameters);
    var jsonResult = await sendMessage(_invokeMethod, message);
    return jsonResult;
  }

  /*
     Dispose of an existing service instance
   */
  Future<bool> dispose(ServiceKey service) async {
    var message = DisposeMessage(service);

    var result = await sendMessage(_disposeMethod, message);

    return true;
  }
}

/*
  Standard class for creation of remote observables along with subscribe and dispose constructs.
*/

class Observables extends RemotingService{
  static const String  _methodChannel = "observables";
  static const String _createMethod = "create";
  static const String _subscribeMethod = "subscribe";
  static const String _disposeMethod = "dispose";

  static String eventChannelName(ServiceKey service) {
    return '${service.value}/events';
  }

  Observables():super(_methodChannel);

  static Observables instance = new Observables();

  /*
    Create an observable service using an existing service method.
   */
  Future<ServiceKey> create(ServiceKey service,String method,List<Parameter> parameters) async {
    var message = new InvokeMessage(service, method, parameters);

    var jsonResult = await sendMessage( _createMethod, message);

    return ServiceKey.fromJson(jsonResult);
  }

  /*
    Subscribe to an existing observable service
   */
  Future<dynamic> subscribe(ServiceKey service) async {
    var message = new ObservableSubscribeMessage(service);
    var result = await sendMessage(_subscribeMethod,message);
  }

  /*
     Dispose of an existing observable service
   */
  Future<bool> dispose(ServiceKey service) async {
    var message = DisposeMessage(service);

    var result = await sendMessage(_disposeMethod, message);

    return true;
  }
}