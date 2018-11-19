import 'package:json_annotation/json_annotation.dart';
import 'serviceKey.dart';

part 'observableNotificationMessage.g.dart';

@JsonSerializable()
class ObservationNotificationMessage
{
  @JsonKey(name:"Service")
  ServiceKey service;

  @JsonKey(name:"Exception")
  String exception;


  @JsonKey(name:"Value")
  String value;

  @JsonKey(name:"Kind")
  NotificationKind kind;


  ObservationNotificationMessage(this.service,this.exception,this.value,this.kind);

  factory ObservationNotificationMessage.fromJson(Map<String, dynamic> json) =>
      _$ObservationNotificationMessageFromJson(json);

  Map<String,dynamic> toJson() => _$ObservationNotificationMessageToJson(this) ;
}

/*
    The differing kinds of observable events that are notified.
 */
enum NotificationKind {OnData,OnError,OnCompleted}
