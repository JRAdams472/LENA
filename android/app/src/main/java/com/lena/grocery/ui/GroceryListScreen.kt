package com.lena.grocery.ui

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Button
import androidx.compose.material3.Checkbox
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.lena.grocery.model.GroceryListItem

@Composable
fun GroceryListScreen(viewModel: GroceryListViewModel) {
    var message by remember { mutableStateOf<String?>(null) }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Grocery List") }
            )
        }
    ) { padding ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(padding)
                .padding(16.dp)
        ) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.spacedBy(8.dp)
            ) {
                OutlinedTextField(
                    value = viewModel.listId.toString(),
                    onValueChange = { text ->
                        text.toIntOrNull()?.let { viewModel.setListId(it) }
                    },
                    label = { Text("List ID") },
                    modifier = Modifier.weight(1f)
                )
                Button(onClick = { viewModel.loadList() }) {
                    Text("Load")
                }
            }

            when {
                viewModel.isLoading -> {
                    Box(
                        modifier = Modifier.fillMaxSize(),
                        contentAlignment = Alignment.Center
                    ) {
                        CircularProgressIndicator()
                    }
                }
                viewModel.errorMessage != null -> {
                    Text(
                        text = viewModel.errorMessage ?: "",
                        color = MaterialTheme.colorScheme.error,
                        modifier = Modifier.padding(top = 16.dp)
                    )
                }
                else -> {
                    LazyColumn(
                        modifier = Modifier.weight(1f),
                        contentPadding = PaddingValues(vertical = 8.dp),
                        verticalArrangement = Arrangement.spacedBy(4.dp)
                    ) {
                        items(viewModel.items, key = { it.groceryListItemID }) { item ->
                            GroceryListItemRow(
                                item = item,
                                onCheckedChange = { viewModel.toggleChecked(item.groceryListItemID) }
                            )
                        }
                    }

                    Button(
                        onClick = {
                            viewModel.save { success, error ->
                                message = if (success) "Saved and stock updated" else error
                            }
                        },
                        modifier = Modifier.fillMaxWidth(),
                        enabled = viewModel.saveState !is GroceryListViewModel.SaveState.Saving
                    ) {
                        Text(
                            when (viewModel.saveState) {
                                is GroceryListViewModel.SaveState.Saving -> "Saving..."
                                else -> "Save"
                            }
                        )
                    }

                    message?.let {
                        Text(
                            text = it,
                            modifier = Modifier.padding(top = 8.dp)
                        )
                    }
                }
            }
        }
    }
}

@Composable
fun GroceryListItemRow(
    item: GroceryListItem,
    onCheckedChange: () -> Unit
) {
    val label = item.itemName
        ?: item.manualItemName
        ?: "Item ${item.itemID}"

    val quantity = buildString {
        append(item.quantityNeeded)
        if (!item.unitOfMeasure.isNullOrBlank()) append(" ").append(item.unitOfMeasure)
    }

    Row(
        modifier = Modifier.fillMaxWidth(),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Checkbox(
            checked = item.isChecked,
            onCheckedChange = { onCheckedChange() }
        )
        Column(modifier = Modifier.weight(1f)) {
            Text(text = label, style = MaterialTheme.typography.bodyLarge)
            Text(text = quantity, style = MaterialTheme.typography.bodySmall)
        }
    }
}
