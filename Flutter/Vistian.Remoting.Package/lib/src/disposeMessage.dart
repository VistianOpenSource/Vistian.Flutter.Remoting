import 'package:json_annotation/json_annotation.dart';
import 'message.dart';
import 'serviceKey.dart';

part 'disposeMessage.g.dart';

@JsonSerializable(createFactory: false)
class DisposeMessage extends Message {
  @JsonKey(name:"Service")
  ServiceKey service;

  DisposeMessage(this.service);

  Map<String,dynamic> toJson() => _$DisposeMessageToJson(this) ;
}
