CREATE PROCEDURE [MealPlan].[usp_GroceryList_ListAll]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT GroceryListID, MealPlanID, GeneratedDate, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[GroceryList]
    ORDER BY GeneratedDate DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [MealPlan].[GroceryList];
END
