CREATE PROCEDURE [MealPlan].[usp_GroceryList_GetById]
    @GroceryListID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT GroceryListID, UserID, MealPlanID, GeneratedDate, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[GroceryList]
    WHERE GroceryListID = @GroceryListID AND UserID = @UserID;
END
