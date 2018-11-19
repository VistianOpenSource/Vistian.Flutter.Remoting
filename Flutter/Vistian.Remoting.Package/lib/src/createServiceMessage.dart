import 'package:json_annotation/json_annotation.dart';
import 'parameter.dart';
import 'message.dart';

part 'createServiceMessage.g.dart';

@JsonSerializable(createFactory: false)
class CreateServiceMessage extends Message {
  @JsonKey(name:"ServiceName")
  final String serviceName;
  @JsonKey(name:"Parameters")
  final List<Parameter> parameters;

  CreateServiceMessage(this.serviceName, this.parameters);

  Map<String,dynamic> toJson() => _$CreateServiceMessageToJson(this) ;
}