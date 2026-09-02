package com.lena.grocery.data

import com.lena.grocery.model.GroceryList
import com.lena.grocery.model.GroceryListItem
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.time.LocalDate
import java.time.format.DateTimeFormatter

class GroceryRepository(baseUrl: String) {

    private val api: ApiService

    init {
        val logging = HttpLoggingInterceptor().apply {
            level = HttpLoggingInterceptor.Level.BODY
        }
        val client = OkHttpClient.Builder()
            .addInterceptor(logging)
            .build()

        api = Retrofit.Builder()
            .baseUrl(baseUrl)
            .client(client)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(ApiService::class.java)
    }

    suspend fun getGroceryList(id: Int): Result<GroceryList> {
        return try {
            val response = api.getGroceryList(id)
            if (response.isSuccessful) {
                Result.success(response.body()!!)
            } else {
                Result.failure(Exception("Failed to load list: ${response.code()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun saveCheckedItems(items: List<GroceryListItem>): Result<Unit> {
        val checked = items.filter { it.isChecked && it.itemID != null }
        val today = LocalDate.now().format(DateTimeFormatter.ISO_DATE)

        return try {
            for (item in checked) {
                val response = api.adjustItemQuantity(
                    itemId = item.itemID!!,
                    quantity = item.quantityNeeded,
                    purchaseDate = today
                )
                if (!response.isSuccessful) {
                    return Result.failure(Exception("Failed to update item ${item.itemID}: ${response.code()}"))
                }
            }
            Result.success(Unit)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}
