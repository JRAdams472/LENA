class GroceryList {
  final int groceryListID;
  final int? mealPlanID;
  final String generatedDate;
  final List<GroceryListItem> groceryListItems;

  const GroceryList({
    required this.groceryListID,
    this.mealPlanID,
    required this.generatedDate,
    required this.groceryListItems,
  });

  factory GroceryList.fromJson(Map<String, dynamic> json) {
    final items = (json['groceryListItems'] as List<dynamic>?)
            ?.map((e) => GroceryListItem.fromJson(e as Map<String, dynamic>))
            .toList() ??
        const [];

    return GroceryList(
      groceryListID: json['groceryListID'] as int,
      mealPlanID: json['mealPlanID'] as int?,
      generatedDate: json['generatedDate'] as String,
      groceryListItems: items,
    );
  }

  GroceryList copyWith({List<GroceryListItem>? groceryListItems}) {
    return GroceryList(
      groceryListID: groceryListID,
      mealPlanID: mealPlanID,
      generatedDate: generatedDate,
      groceryListItems: groceryListItems ?? this.groceryListItems,
    );
  }
}

class GroceryListItem {
  final int groceryListItemID;
  final int groceryListID;
  final int? itemID;
  final String? itemName;
  final String? manualItemName;
  final double quantityNeeded;
  final String? unitOfMeasure;
  final String source;
  bool isChecked;

  GroceryListItem({
    required this.groceryListItemID,
    required this.groceryListID,
    this.itemID,
    this.itemName,
    this.manualItemName,
    required this.quantityNeeded,
    this.unitOfMeasure,
    required this.source,
    this.isChecked = false,
  });

  factory GroceryListItem.fromJson(Map<String, dynamic> json) {
    return GroceryListItem(
      groceryListItemID: json['groceryListItemID'] as int,
      groceryListID: json['groceryListID'] as int,
      itemID: json['itemID'] as int?,
      itemName: json['itemName'] as String?,
      manualItemName: json['manualItemName'] as String?,
      quantityNeeded: (json['quantityNeeded'] as num).toDouble(),
      unitOfMeasure: json['unitOfMeasure'] as String?,
      source: json['source'] as String,
      isChecked: json['isChecked'] as bool? ?? false,
    );
  }

  String get displayName => itemName ?? manualItemName ?? 'Unknown item';
}
