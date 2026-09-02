CREATE PROCEDURE [MealPlan].[usp_MealPlan_ListAll]
    @UserID INT,
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT MealPlanID, UserID, PlanName, WeekStartDate, WeekStartDayOfWeek, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[MealPlan]
    WHERE UserID = @UserID
    ORDER BY PlanName
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [MealPlan].[MealPlan] WHERE UserID = @UserID;
END
