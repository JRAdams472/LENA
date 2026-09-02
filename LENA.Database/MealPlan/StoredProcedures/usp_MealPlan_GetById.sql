CREATE PROCEDURE [MealPlan].[usp_MealPlan_GetById]
    @MealPlanID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MealPlanID, UserID, PlanName, WeekStartDate, WeekStartDayOfWeek, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[MealPlan]
    WHERE MealPlanID = @MealPlanID AND UserID = @UserID;
END
