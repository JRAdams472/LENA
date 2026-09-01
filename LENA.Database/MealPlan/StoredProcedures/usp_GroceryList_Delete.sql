CREATE PROCEDURE [MealPlan].[usp_GroceryList_Delete]
    @GroceryListID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [MealPlan].[GroceryList]
    WHERE GroceryListID = @GroceryListID;

    SELECT @@ROWCOUNT;
END
