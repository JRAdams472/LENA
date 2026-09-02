CREATE PROCEDURE [MealPlan].[usp_GroceryList_Delete]
    @GroceryListID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [MealPlan].[GroceryList]
    WHERE GroceryListID = @GroceryListID AND UserID = @UserID;

    SELECT @@ROWCOUNT;
END
