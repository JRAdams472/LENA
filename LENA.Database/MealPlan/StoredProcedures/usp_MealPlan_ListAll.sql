CREATE PROCEDURE [MealPlan].[usp_MealPlan_ListAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MealPlanID, PlanName, WeekStartDate, WeekStartDayOfWeek, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[MealPlan];
END
