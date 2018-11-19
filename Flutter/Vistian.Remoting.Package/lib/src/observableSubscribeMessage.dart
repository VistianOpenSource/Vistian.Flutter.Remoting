import 'package:json_annotation/json_annotation.dart';
import 'serviceKey.dart';
import 'message.dart';
/*
    Subscribe to a remote observable
 */

part 'observableSubscribeMessage.g.dart';

@JsonSerializable(createFactory: false)
class ObservableSubscribeMessage extends Message{
  @JsonKey(name:"Service")
  ServiceKey serviceKey;

  ObservableSubscribeMessage(this.serviceKey);

  Map<String,dynamic> toJson() => _$ObservableSubscribeMessageToJson(this) ;
}
