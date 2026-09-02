CREATE PROCEDURE [MealPlan].[usp_MealPlan_GetByName]
    @PlanName NVARCHAR(200),
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MealPlanID, UserID, PlanName, WeekStartDate, WeekStartDayOfWeek, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[MealPlan]
    WHERE PlanName = @PlanName AND UserID = @UserID;
END
