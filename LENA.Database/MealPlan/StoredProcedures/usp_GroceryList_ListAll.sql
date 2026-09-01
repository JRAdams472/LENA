CREATE PROCEDURE [MealPlan].[usp_GroceryList_ListAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT GroceryListID, MealPlanID, GeneratedDate, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[GroceryList]
    ORDER BY GeneratedDate DESC;
END
