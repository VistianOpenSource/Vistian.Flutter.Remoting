
import 'package:json_annotation/json_annotation.dart';

part 'serviceKey.g.dart';

@JsonSerializable()
class ServiceKey extends Object {
  @JsonKey(name:'Value')
  String value;

  ServiceKey(this.value);

  factory ServiceKey.fromJson(Map<String, dynamic> json) => _$ServiceKeyFromJson(json);

  Map<String,dynamic> toJson() => _$ServiceKeyToJson(this);
}