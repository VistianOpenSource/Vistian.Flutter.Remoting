
/*
  A proxy observable which uses and EventChannel to receive data from and then publish that data as if it were the source
  of that data.
 */
import 'dart:async';
import 'dart:convert';
import 'package:rxdart/rxdart.dart';
import 'package:flutter/services.dart';

import 'observableNotificationMessage.dart';
import 'serviceRemoting.dart';
import 'resultConverter.dart';
import 'serviceKey.dart';

class ObservableProxy<T> extends Observable<T>  {
  final StreamController<T> _controller;
  final EventChannel _eventChannel;
  final ServiceKey service;
  StreamSubscription _disposable;
  final ResultConverter _resultConverter;

  ObservableProxy._(this._eventChannel,this._controller, this.service,this._resultConverter) : super(_controller.stream){
    this._controller.onListen = _onListenCallback;
    this._controller.onCancel = _onCancelCallback;

    _disposable = _eventChannel.receiveBroadcastStream().listen(onNotificationData);
  }

  /*
      The observable has been listened to for the first time. Create a subscribe message and pass it over
      via remoting to the host system.
   */
  Future<dynamic> _onListenCallback  () async {
    var result = Observables.instance.subscribe(service);

    return result;
  }

  /*
     The last of the listeners to the observable have gone away. Create a dispose message letting the host
     end.
   */
  Future _onCancelCallback() async {
    await Observables.instance.dispose(service);
  }


  /*
      Data has been received from the event Channel.
   */
  void onNotificationData(dynamic notification){
    var jsonMessage = json.decode(notification);

    // extract the message from the received json
    var message = ObservationNotificationMessage.fromJson(jsonMessage);

    // act depending upon type of data received.
    switch(message.kind){
      case NotificationKind.OnCompleted:
        onComplete();
        break;

      case NotificationKind.OnData:
        var data = json.decode(message.value);
        var obj = _resultConverter(data);
        onData(obj);
        break;

      case NotificationKind.OnError:
        onError(message.exception);
        break;
    }
  }

  @override
  void onComplete() {
    _controller.close();
    _disposable.cancel();
  }

  @override
  void onData(Object data) {
    _controller.add(data);
  }

  @override
  void onError(String error) {
    _controller.addError(error);
    _disposable.cancel();
  }

  //  Create an observable proxy associated with a specified instance together with listen and cancel callbacks
  factory ObservableProxy.create(ServiceKey service,ResultConverter resultConverter){
    //
    var eventStream = EventChannel(Observables.eventChannelName(service));
    var controller = new StreamController<T>.broadcast();
    return new ObservableProxy<T>._(eventStream,controller,service,resultConverter);
  }
}
