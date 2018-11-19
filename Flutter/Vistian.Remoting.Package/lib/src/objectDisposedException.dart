/*
    Raised when an object is disposed and operations are being attempted against it
 */
import 'service.dart';

class ObjectDisposedException implements Exception{
  // the associated service
  final Service service;

  ObjectDisposedException(this.service);
}
