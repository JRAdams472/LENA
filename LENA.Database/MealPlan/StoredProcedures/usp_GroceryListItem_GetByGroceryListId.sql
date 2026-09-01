CREATE PROCEDURE [MealPlan].[usp_GroceryListItem_GetByGroceryListId]
    @GroceryListID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT GroceryListItemID, GroceryListID, ItemID, ManualItemName, QuantityNeeded, UnitOfMeasure, Source, IsChecked, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[GroceryListItem]
    WHERE GroceryListID = @GroceryListID
    ORDER BY Source, ManualItemName;
END
