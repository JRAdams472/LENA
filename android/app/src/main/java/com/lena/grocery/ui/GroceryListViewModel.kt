package com.lena.grocery.ui

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.lena.grocery.data.GroceryRepository
import com.lena.grocery.model.GroceryList
import com.lena.grocery.model.GroceryListItem
import kotlinx.coroutines.launch

class GroceryListViewModel(private val repository: GroceryRepository) : ViewModel() {

    var isLoading by mutableStateOf(false)
        private set

    var errorMessage by mutableStateOf<String?>(null)
        private set

    var saveState by mutableStateOf<SaveState>(SaveState.Idle)
        private set

    var listId by mutableStateOf(1)
        private set

    private var _groceryList: GroceryList? = null
    val items = mutableStateListOf<GroceryListItem>()

    fun setListId(id: Int) {
        listId = id
    }

    fun loadList() {
        viewModelScope.launch {
            isLoading = true
            errorMessage = null

            repository.getGroceryList(listId)
                .onSuccess { list ->
                    _groceryList = list
                    items.clear()
                    items.addAll(list.groceryListItems)
                }
                .onFailure { error ->
                    errorMessage = error.localizedMessage ?: "Failed to load grocery list"
                }

            isLoading = false
        }
    }

    fun toggleChecked(itemId: Int) {
        val index = items.indexOfFirst { it.groceryListItemID == itemId }
        if (index != -1) {
            items[index] = items[index].copy(isChecked = !items[index].isChecked)
        }
    }

    fun save(onComplete: (Boolean, String?) -> Unit) {
        viewModelScope.launch {
            saveState = SaveState.Saving

            repository.saveCheckedItems(items.toList())
                .onSuccess {
                    saveState = SaveState.Saved
                    onComplete(true, null)
                }
                .onFailure { error ->
                    val message = error.localizedMessage ?: "Save failed"
                    saveState = SaveState.Error(message)
                    onComplete(false, message)
                }
        }
    }

    sealed class SaveState {
        object Idle : SaveState()
        object Saving : SaveState()
        object Saved : SaveState()
        data class Error(val message: String) : SaveState()
    }

    @Suppress("UNCHECKED_CAST")
    class Factory(private val repository: GroceryRepository) : ViewModelProvider.Factory {
        override fun <T : ViewModel> create(modelClass: Class<T>): T {
            return GroceryListViewModel(repository) as T
        }
    }
}
