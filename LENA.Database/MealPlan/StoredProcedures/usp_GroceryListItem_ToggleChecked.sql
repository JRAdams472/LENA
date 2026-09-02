CREATE PROCEDURE [MealPlan].[usp_GroceryListItem_ToggleChecked]
    @GroceryListItemID INT,
    @UserID INT,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE gli
    SET IsChecked = CASE WHEN IsChecked = 1 THEN 0 ELSE 1 END,
        LastUpdatedBy = @LastUpdatedBy,
        LastUpdatedDate = @LastUpdatedDate
    FROM [MealPlan].[GroceryListItem] gli
    INNER JOIN [MealPlan].[GroceryList] gl ON gli.GroceryListID = gl.GroceryListID
    WHERE gli.GroceryListItemID = @GroceryListItemID AND gl.UserID = @UserID;

    SELECT gli.IsChecked
    FROM [MealPlan].[GroceryListItem] gli
    INNER JOIN [MealPlan].[GroceryList] gl ON gli.GroceryListID = gl.GroceryListID
    WHERE gli.GroceryListItemID = @GroceryListItemID AND gl.UserID = @UserID;
END
