class User {
  final int userID;
  final String email;
  final String? displayName;
  final String? externalSubject;
  final String? provider;

  const User({
    required this.userID,
    required this.email,
    this.displayName,
    this.externalSubject,
    this.provider,
  });

  factory User.fromJson(Map<String, dynamic> json) {
    return User(
      userID: json['userID'] as int,
      email: json['email'] as String,
      displayName: json['displayName'] as String?,
      externalSubject: json['externalSubject'] as String?,
      provider: json['provider'] as String?,
    );
  }
}
