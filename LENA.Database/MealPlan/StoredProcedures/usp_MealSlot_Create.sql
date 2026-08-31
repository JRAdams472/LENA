CREATE PROCEDURE [MealPlan].[usp_MealSlot_Create]
    @MealPlanID INT,
    @DayOfWeek TINYINT,
    @MealType TINYINT,
    @RecipeID INT = NULL,
    @ReplacementNote NVARCHAR(500) = NULL,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [MealPlan].[MealSlot] (MealPlanID, DayOfWeek, MealType, RecipeID, ReplacementNote, CreatedBy, CreateDate)
    VALUES (@MealPlanID, @DayOfWeek, @MealType, @RecipeID, @ReplacementNote, @CreatedBy, @CreateDate);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
