class RemoteTypeMapper{

  final Map<String,String> _mappings;

  RemoteTypeMapper(this._mappings);

  String getRemoteType(Type dartType){

    if (!_mappings.containsKey(dartType.toString())){
      print("missing type ${dartType.toString()}");
      return "";
    }
    return _mappings[dartType.toString()];
  }
}