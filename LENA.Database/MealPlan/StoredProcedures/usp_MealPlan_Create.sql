CREATE PROCEDURE [MealPlan].[usp_MealPlan_Create]
    @PlanName NVARCHAR(200),
    @WeekStartDate DATE,
    @WeekStartDayOfWeek TINYINT = 0,
    @IsActive BIT = 1,
    @UserID INT,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [MealPlan].[MealPlan] (PlanName, WeekStartDate, WeekStartDayOfWeek, IsActive, UserID, CreatedBy, CreateDate)
    VALUES (@PlanName, @WeekStartDate, @WeekStartDayOfWeek, @IsActive, @UserID, @CreatedBy, @CreateDate);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
