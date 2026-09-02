CREATE PROCEDURE [MealPlan].[usp_GroceryList_ListAll]
    @UserID INT,
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT GroceryListID, UserID, MealPlanID, GeneratedDate, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[GroceryList]
    WHERE UserID = @UserID
    ORDER BY GeneratedDate DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [MealPlan].[GroceryList] WHERE UserID = @UserID;
END
