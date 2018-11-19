// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'observableNotificationMessage.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ObservationNotificationMessage _$ObservationNotificationMessageFromJson(
    Map<String, dynamic> json) {
  return ObservationNotificationMessage(
      json['Service'] == null
          ? null
          : ServiceKey.fromJson(json['Service'] as Map<String, dynamic>),
      json['Exception'] as String,
      json['Value'] as String,
      _$enumDecodeNullable(_$NotificationKindEnumMap, json['Kind']));
}

Map<String, dynamic> _$ObservationNotificationMessageToJson(
        ObservationNotificationMessage instance) =>
    <String, dynamic>{
      'Service': instance.service,
      'Exception': instance.exception,
      'Value': instance.value,
      'Kind': _$NotificationKindEnumMap[instance.kind]
    };

T _$enumDecode<T>(Map<T, dynamic> enumValues, dynamic source) {
  if (source == null) {
    throw ArgumentError('A value must be provided. Supported values: '
        '${enumValues.values.join(', ')}');
  }
  return enumValues.entries
      .singleWhere((e) => e.value == source,
          orElse: () => throw ArgumentError(
              '`$source` is not one of the supported values: '
              '${enumValues.values.join(', ')}'))
      .key;
}

T _$enumDecodeNullable<T>(Map<T, dynamic> enumValues, dynamic source) {
  if (source == null) {
    return null;
  }
  return _$enumDecode<T>(enumValues, source);
}

const _$NotificationKindEnumMap = <NotificationKind, dynamic>{
  NotificationKind.OnData: 'OnData',
  NotificationKind.OnError: 'OnError',
  NotificationKind.OnCompleted: 'OnCompleted'
};
