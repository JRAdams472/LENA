import 'package:flutter/material.dart';

import '../services/api_service.dart';
import '../services/auth_service.dart';

class LoginScreen extends StatefulWidget {
  final AuthService authService;
  final ApiService apiService;

  const LoginScreen({
    super.key,
    required this.authService,
    required this.apiService,
  });

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  bool _isLoading = false;

  Future<void> _signIn() async {
    setState(() => _isLoading = true);

    try {
      final token = await widget.authService.signIn();
      if (token == null || !mounted) {
        setState(() => _isLoading = false);
        return;
      }

      // Verify the token with the API and load the current user.
      await widget.apiService.getMe();

      if (mounted) {
        Navigator.pushReplacementNamed(context, '/');
      }
    } on StateError catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text(e.message)),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Sign in failed: $e')),
        );
      }
    } finally {
      if (mounted) {
        setState(() => _isLoading = false);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('LENA Sign In')),
      body: Center(
        child: _isLoading
            ? const CircularProgressIndicator()
            : ElevatedButton.icon(
                icon: const Icon(Icons.login),
                label: const Text('Sign in with Google'),
                onPressed: _signIn,
              ),
      ),
    );
  }
}
