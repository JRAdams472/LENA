CREATE PROCEDURE [MealPlan].[usp_GroceryListItem_GetByGroceryListId]
    @GroceryListID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT gli.GroceryListItemID, gli.GroceryListID, gli.ItemID, i.Name AS ItemName, gli.ManualItemName,
           gli.QuantityNeeded, gli.UnitOfMeasure, gli.Source, gli.IsChecked,
           gli.CreatedBy, gli.CreateDate, gli.LastUpdatedBy, gli.LastUpdatedDate
    FROM [MealPlan].[GroceryListItem] gli
    INNER JOIN [MealPlan].[GroceryList] gl ON gli.GroceryListID = gl.GroceryListID
    LEFT JOIN [Inventory].[Item] i ON gli.ItemID = i.ItemID
    WHERE gli.GroceryListID = @GroceryListID AND gl.UserID = @UserID
    ORDER BY gli.Source, COALESCE(i.Name, gli.ManualItemName);
END
