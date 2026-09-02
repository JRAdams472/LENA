package com.lena.grocery.model

data class User(
    val userID: Int,
    val email: String,
    val displayName: String? = null,
    val externalSubject: String? = null,
    val provider: String? = null
)
