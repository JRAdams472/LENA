CREATE PROCEDURE [MealPlan].[usp_MealPlan_ListAllPaged]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT MealPlanID, PlanName, WeekStartDate, WeekStartDayOfWeek, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[MealPlan]
    ORDER BY PlanName
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [MealPlan].[MealPlan];
END
