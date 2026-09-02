import 'package:flutter/material.dart';

import 'screens/home_screen.dart';
import 'screens/login_screen.dart';
import 'screens/grocery_list_screen.dart';
import 'services/api_service.dart';
import 'services/auth_service.dart';

final _navigatorKey = GlobalKey<NavigatorState>();
final _authService = AuthService();
late final ApiService _apiService;

void main() {
  _apiService = ApiService(
    _authService,
    onUnauthorized: () {
      _navigatorKey.currentState?.pushReplacementNamed('/login');
    },
  );

  runApp(const LenaApp());
}

class LenaApp extends StatelessWidget {
  const LenaApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'LENA',
      navigatorKey: _navigatorKey,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
        useMaterial3: true,
      ),
      initialRoute: '/login',
      routes: {
        '/login': (context) => LoginScreen(authService: _authService, apiService: _apiService),
        '/': (context) => HomeScreen(authService: _authService, apiService: _apiService),
        '/grocery-list': (context) => GroceryListScreen(apiService: _apiService),
      },
    );
  }
}
