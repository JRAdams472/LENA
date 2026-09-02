package com.lena.grocery.data

import com.lena.grocery.model.GroceryList
import com.lena.grocery.model.User
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.Query

data class QuantityAdjustment(
    val quantity: Double,
    val purchaseDate: String? = null
)

interface ApiService {
    @GET("api/auth/me")
    suspend fun getMe(): Response<User>

    @GET("api/GroceryList/{id}")
    suspend fun getGroceryList(@Path("id") id: Int): Response<GroceryList>

    @POST("api/Item/items/{id}/quantity")
    suspend fun adjustItemQuantity(
        @Path("id") itemId: Int,
        @Query("quantity") quantity: Double,
        @Query("purchaseDate") purchaseDate: String? = null
    ): Response<Unit>
}
