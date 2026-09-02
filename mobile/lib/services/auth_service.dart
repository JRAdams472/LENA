import 'dart:async';
import 'dart:convert';

import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:google_sign_in/google_sign_in.dart';

import '../config/api_config.dart';

class AuthService {
  static const _tokenKey = 'lena_id_token';
  static const _storage = FlutterSecureStorage();

  GoogleSignIn? _googleSignIn;

  Future<String?> getToken() async {
    final token = await _storage.read(key: _tokenKey);
    if (token == null) return null;

    if (_isTokenExpired(token)) {
      await signOut();
      return null;
    }

    return token;
  }

  Future<bool> isSignedIn() async {
    return (await getToken()) != null;
  }

  Future<String?> signIn() async {
    const placeholder = '__YOUR_GOOGLE_CLIENT_ID__';
    if (ApiConfig.googleServerClientId.isEmpty ||
        ApiConfig.googleServerClientId == placeholder) {
      throw StateError(
        'GOOGLE_SERVER_CLIENT_ID is not configured. '
        'Provide it via --dart-define=GOOGLE_SERVER_CLIENT_ID=<your-web-client-id>',
      );
    }

    _googleSignIn ??= GoogleSignIn(
      scopes: ['openid', 'email', 'profile'],
      serverClientId: ApiConfig.googleServerClientId,
    );

    var account = await _googleSignIn!.signInSilently();
    account ??= await _googleSignIn!.signIn();

    final auth = await account?.authentication;
    final idToken = auth?.idToken;

    if (idToken != null) {
      await _storage.write(key: _tokenKey, value: idToken);
      return idToken;
    }

    return null;
  }

  Future<void> signOut() async {
    try {
      await _googleSignIn?.signOut();
    } catch (_) {
      // best effort
    }
    await _storage.delete(key: _tokenKey);
  }

  bool _isTokenExpired(String token) {
    try {
      final parts = token.split('.');
      if (parts.length != 3) return true;

      final normalized = base64Url.normalize(parts[1]);
      final payload = utf8.decode(base64Url.decode(normalized));
      final map = json.decode(payload) as Map<String, dynamic>;
      final exp = map['exp'] as int?;

      if (exp == null) return false;

      return DateTime.now().millisecondsSinceEpoch ~/ 1000 >= exp;
    } catch (_) {
      return true;
    }
  }
}
