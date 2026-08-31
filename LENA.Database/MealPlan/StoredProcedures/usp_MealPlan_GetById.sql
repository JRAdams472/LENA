CREATE PROCEDURE [MealPlan].[usp_MealPlan_GetById]
    @MealPlanID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MealPlanID, PlanName, WeekStartDate, WeekStartDayOfWeek, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[MealPlan]
    WHERE MealPlanID = @MealPlanID;
END
