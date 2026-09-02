import 'dart:convert';

import 'package:http/http.dart' as http;

import '../config/api_config.dart';
import '../models/grocery_list.dart';
import '../models/user.dart';
import 'auth_service.dart';

class ApiService {
  final AuthService _authService;
  final VoidCallback onUnauthorized;

  ApiService(this._authService, {required this.onUnauthorized});

  Future<http.Response> get(String path) => _request('GET', path);

  Future<http.Response> post(String path, {Object? body}) =>
      _request('POST', path, body: body);

  Future<http.Response> put(String path, {Object? body}) =>
      _request('PUT', path, body: body);

  Future<http.Response> delete(String path) => _request('DELETE', path);

  Future<User> getMe() async {
    final response = await get('/api/auth/me');
    _expectSuccess(response);
    return User.fromJson(json.decode(response.body) as Map<String, dynamic>);
  }

  Future<GroceryList> getGroceryList(int id) async {
    final response = await get('/api/GroceryList/$id');
    _expectSuccess(response);
    return GroceryList.fromJson(json.decode(response.body) as Map<String, dynamic>);
  }

  Future<void> adjustItemQuantity(
    int itemId,
    double quantity, {
    DateTime? purchaseDate,
  }) async {
    final query = <String, String>{'quantity': quantity.toString()};
    if (purchaseDate != null) {
      query['purchaseDate'] = purchaseDate.toUtc().toIso8601String();
    }
    final queryString = _encodeQuery(query);
    final response = await post('/api/Item/items/$itemId/quantity?$queryString');
    _expectSuccess(response);
  }

  Future<http.Response> _request(
    String method,
    String path, {
    Object? body,
  }) async {
    final token = await _authService.getToken();
    final uri = Uri.parse('${ApiConfig.baseUrl}$path');
    final headers = <String, String>{
      'Accept': 'application/json',
    };

    if (token != null) {
      headers['Authorization'] = 'Bearer $token';
    }

    String? encodedBody;
    if (body != null) {
      headers['Content-Type'] = 'application/json';
      encodedBody = json.encode(body);
    }

    final request = http.Request(method, uri)..headers.addAll(headers);
    if (encodedBody != null) {
      request.body = encodedBody;
    }

    final streamed = await request.send();
    final response = await http.Response.fromStream(streamed);

    if (response.statusCode == 401) {
      await _authService.signOut();
      onUnauthorized();
    }

    return response;
  }

  void _expectSuccess(http.Response response) {
    if (response.statusCode < 200 || response.statusCode >= 300) {
      throw Exception('Request failed: ${response.statusCode} ${response.body}');
    }
  }

  String _encodeQuery(Map<String, String> params) {
    return params.entries
        .map((e) => '${Uri.encodeComponent(e.key)}=${Uri.encodeComponent(e.value)}')
        .join('&');
  }
}

typedef VoidCallback = void Function();
