/*
  A parameter passed to a remoting service.
 */
import 'dart:convert';
import 'package:json_annotation/json_annotation.dart';

part 'parameter.g.dart';

@JsonSerializable(createFactory: false)
class Parameter{
  // the name of the parameter
  @JsonKey(name:'Name')
  final String name;
  // the actual value to be used for the parameter
  @JsonKey(name:'Value')
  final String value;
  // the value of the parameter
  @JsonKey(name:'ValueType')
  final String valueType;

  Parameter(this.name,dynamic value,this.valueType):value = json.encode(value);

  Map<String,dynamic> toJson() => _$ParameterToJson(this) ;
}