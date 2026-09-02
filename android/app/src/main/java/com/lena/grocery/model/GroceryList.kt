package com.lena.grocery.model

data class GroceryList(
    val groceryListID: Int,
    val mealPlanID: Int?,
    val generatedDate: String?,
    val groceryListItems: List<GroceryListItem>
)

data class GroceryListItem(
    val groceryListItemID: Int,
    val groceryListID: Int,
    val itemID: Int?,
    val itemName: String?,
    val manualItemName: String?,
    val quantityNeeded: Double,
    val unitOfMeasure: String?,
    val source: String,
    var isChecked: Boolean
)
