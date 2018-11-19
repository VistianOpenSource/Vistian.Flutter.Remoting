/*
  Abstract class from which all classes which provide remoting services must derive from.
 */
import 'dart:async';
import 'package:flutter/services.dart';
import 'package:rxdart/rxdart.dart';

import 'parameter.dart';
import 'resultConverter.dart';
import 'objectDisposedException.dart';
import 'observableProxy.dart';
import 'remoteTypeMapper.dart';
import 'service.dart';
import 'serviceKey.dart';
import 'serviceRemoting.dart';

abstract class Remoting {
  // the associated service
  ServiceKey _service;

  // current state of this proxy
  RemotingState _state = RemotingState.pendingInitialization;

  // Get the associated service - overridden by derived classes.
  Service get service;

  // get the remote type mapper
  RemoteTypeMapper get remoteTypeMapper;
  /*
    Invoke the specified method with the parameters and the 'converter' function
    to take the results of the invocation and
   */
  Future<dynamic> $_invoke<T>(String method, {Map params, T converter(p)}) async {
    // verify we are initialized 1st
    await _checkInitialized();
    try {
      var parameters = createParameters(params);
      var jsonResult = await Services.instance.invoke(_service, method, parameters);
      var converted = converter(jsonResult);

      return converted;
    }
    on MissingPluginException catch (e) {
      print("More than likely the native code to catch the requests is missing");
      print("Make sure you have your ProxyHandler setup in the native code");
      throw e;
    }
    catch (e) {
      // better be careful, the 'you shouldn't just catch exceptions cops' will be out.
      // I say if its good enough for firebase its good enough for me.
      print("_invokeError:" + e.toString());
      throw e;
    }
  }

  /*
    Invoke the specified method which results in an observable being returned.
   */
  Future<Observable<T>> $_invokeObservable<T>(String method,
      {Map params, ResultConverter converter}) async {
    await _checkInitialized();
    try{

      var parameters = createParameters(params);
      var service = await Observables.instance.create(_service, method, parameters);
      var observableProxy = ObservableProxy<T>.create(service,converter);
    return observableProxy;
    }
    on MissingPluginException catch (e) {
    print("More than likely the native code to catch the requests is missing");
    print("Make sure you have your ProxyHandler setup in the native code");

    throw e;
    }

    catch (e) {
    // a more general error,
    print(" ${e.toString()}");
    throw e;
    }
  }

  /*
      Check if we are initialized, if we aren't then we need to
      perform the initialization phase.
   */
  Future<void> _checkInitialized({bool initialize=true}) async {
    if (_state == RemotingState.initialized){
      return;
    }

    if (_state == RemotingState.initializing){
      await _initializingCompleter.future;
      return;
    }

    if (_state == RemotingState.disposed){
      throw new ObjectDisposedException(service);
    }


    if (_state == RemotingState.pendingInitialization && initialize){
      await _initialize();
    }
  }


  // used for unlikely situation of people awaiting multiple
  // calls at the same time from this class whilst initializing
  Completer _initializingCompleter = new Completer();

  Future<bool> _initialize() async {
    // need to chat to the 'other side' to get our channel information
    // not initialized, then we need to chat via the platform channel to initialize

    // we invoke and this gives us the instance identifier...

    _state = RemotingState.initializing;

    try {
      var parameters = createParameters(initializationParameters());
      var serviceKey = await Services.instance.create(service, parameters);

      _service = serviceKey;
      _state = RemotingState.initialized;
      _initializingCompleter.complete(true);

      return true;
    }
    on MissingPluginException catch (e) {
      print(
          "More than likely the native code to catch the requests is missing");
      print("Make sure you have your ProxyHandler setup in the native code");

      _initializingCompleter.completeError(e);
      throw e;
    }

    catch (e) {
      _initializingCompleter.completeError(e);
      print("_initialize " + e.toString());
    }

    return true;
  }

  /*
    For a map of parameters (and their values) construct a list of parameters
    with hopefully remote type details also being provided.
   */
  List<Parameter> createParameters(Map<String,dynamic> rawParameters){
    var parameters = rawParameters.entries.map((f)  {
      var mappedType = remoteTypeMapper.getRemoteType(f.value.runtimeType);
      var parameter = Parameter(f.key, f.value,mappedType);
      return parameter;
    }).toList();

    return parameters;
  }


  /*
     Overridden in inherited classes to specify optional initialization parameter that
     are used as part of the initialization phase.
    */

  Map<String, dynamic> initializationParameters() => <String, dynamic>{};

  // everything is done with, time to do.
  Future<bool> _destroy() async {
    await _checkInitialized(initialize: false);

    try {
      await Services.instance.dispose(_service);
    }
    catch (e) {
      // just record the error and thats it
    }
    finally {
      _state = RemotingState.disposed;
    }

    return true;
  }

  void dispose() {
    _destroy();
  }
}

/*
   The state of remoting proxy classs
 */
enum RemotingState {pendingInitialization,initializing,initialized,disposed}
