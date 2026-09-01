CREATE PROCEDURE [MealPlan].[usp_GroceryListItem_Delete]
    @GroceryListItemID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [MealPlan].[GroceryListItem]
    WHERE GroceryListItemID = @GroceryListItemID;

    SELECT @@ROWCOUNT;
END
