
import 'remoteServices.dart';

class TestService extends $_ITestServiceMixin {

  // mechanism used to provide 'constructor' details for a remote service
  @override
  Map<String,dynamic > initializationParameters() {
    return <String,dynamic>{'settings' : new ServiceSettings(100,200)};
  }
}