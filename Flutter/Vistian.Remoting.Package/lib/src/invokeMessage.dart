import 'package:json_annotation/json_annotation.dart';
import 'message.dart';
import 'parameter.dart';
import 'serviceKey.dart';


part 'invokeMessage.g.dart';

@JsonSerializable(createFactory: false)
class InvokeMessage extends Message{
  @JsonKey(name:"Service")
  ServiceKey service;
  @JsonKey(name:"Method")
  String method;
  @JsonKey(name:"Parameters")
  List<Parameter> parameters;

  InvokeMessage(this.service,this.method,this.parameters);

  Map<String,dynamic> toJson() => _$InvokeMessageToJson(this) ;
}
