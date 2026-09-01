CREATE PROCEDURE [MealPlan].[usp_GroceryListItem_ToggleChecked]
    @GroceryListItemID INT,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [MealPlan].[GroceryListItem]
    SET IsChecked = CASE WHEN IsChecked = 1 THEN 0 ELSE 1 END,
        LastUpdatedBy = @LastUpdatedBy,
        LastUpdatedDate = @LastUpdatedDate
    WHERE GroceryListItemID = @GroceryListItemID;

    SELECT IsChecked
    FROM [MealPlan].[GroceryListItem]
    WHERE GroceryListItemID = @GroceryListItemID;
END
