class ApiConfig {
  /// API base URL. Defaults to the Android emulator loopback address.
  /// Override at build time with --dart-define=API_BASE_URL=...
  static const baseUrl = String.fromEnvironment(
    'API_BASE_URL',
    defaultValue: 'http://10.0.2.2:5059',
  );

  /// Google Web/Server client ID used to obtain an ID token with the correct audience.
  /// This must match the API's Authentication:Google:ClientId value.
  static const googleServerClientId = String.fromEnvironment(
    'GOOGLE_SERVER_CLIENT_ID',
    defaultValue: '',
  );
}
