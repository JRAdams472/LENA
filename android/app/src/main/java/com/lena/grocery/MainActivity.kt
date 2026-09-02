package com.lena.grocery

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.compose.material3.MaterialTheme
import com.lena.grocery.data.GroceryRepository
import com.lena.grocery.ui.GroceryListScreen
import com.lena.grocery.ui.GroceryListViewModel

class MainActivity : ComponentActivity() {

    private val viewModel: GroceryListViewModel by viewModels {
        GroceryListViewModel.Factory(
            GroceryRepository(BuildConfig.API_BASE_URL)
        )
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            MaterialTheme {
                GroceryListScreen(viewModel)
            }
        }
    }
}
