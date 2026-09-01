CREATE PROCEDURE [MealPlan].[usp_MealPlan_GetByName]
    @PlanName NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MealPlanID, PlanName, WeekStartDate, WeekStartDayOfWeek, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[MealPlan]
    WHERE PlanName = @PlanName;
END
