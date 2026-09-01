CREATE PROCEDURE [MealPlan].[usp_GroceryList_GetById]
    @GroceryListID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT GroceryListID, MealPlanID, GeneratedDate, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[GroceryList]
    WHERE GroceryListID = @GroceryListID;
END
