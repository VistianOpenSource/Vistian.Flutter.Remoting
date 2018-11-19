import 'package:json_annotation/json_annotation.dart';

part 'messagingResult.g.dart';

@JsonSerializable()
class MessagingResult extends Object{

  @JsonKey(name:'JsonResult')
  String jsonResult;

  MessagingResult(this.jsonResult);

  factory MessagingResult.fromJson(Map<String,dynamic> json) => _$MessagingResultFromJson(json);
}