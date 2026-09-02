CREATE PROCEDURE [MealPlan].[usp_GroceryListItem_Delete]
    @GroceryListItemID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE gli
    FROM [MealPlan].[GroceryListItem] gli
    INNER JOIN [MealPlan].[GroceryList] gl ON gli.GroceryListID = gl.GroceryListID
    WHERE gli.GroceryListItemID = @GroceryListItemID AND gl.UserID = @UserID;

    SELECT @@ROWCOUNT;
END
