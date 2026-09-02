CREATE PROCEDURE [MealPlan].[usp_MealPlan_Update]
    @MealPlanID INT,
    @PlanName NVARCHAR(200),
    @WeekStartDate DATE,
    @WeekStartDayOfWeek TINYINT,
    @IsActive BIT = 1,
    @UserID INT,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [MealPlan].[MealPlan]
    SET PlanName = @PlanName,
        WeekStartDate = @WeekStartDate,
        WeekStartDayOfWeek = @WeekStartDayOfWeek,
        IsActive = @IsActive,
        LastUpdatedBy = @LastUpdatedBy,
        LastUpdatedDate = @LastUpdatedDate
    WHERE MealPlanID = @MealPlanID AND UserID = @UserID;

    SELECT @@ROWCOUNT;
END
