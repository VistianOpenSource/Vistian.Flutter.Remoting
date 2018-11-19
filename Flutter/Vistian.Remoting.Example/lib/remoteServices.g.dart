// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'remoteServices.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Location _$LocationFromJson(Map<String, dynamic> json) {
  return Location((json['Latitude'] as num)?.toDouble(),
      (json['Longitude'] as num)?.toDouble());
}

Map<String, dynamic> _$LocationToJson(Location instance) => <String, dynamic>{
      'Latitude': instance.latitude,
      'Longitude': instance.longitude
    };

ServiceSettings _$ServiceSettingsFromJson(Map<String, dynamic> json) {
  return ServiceSettings(json['Tolerance'] as int, json['Period'] as int);
}

Map<String, dynamic> _$ServiceSettingsToJson(ServiceSettings instance) =>
    <String, dynamic>{
      'Tolerance': instance.tolerance,
      'Period': instance.period
    };
