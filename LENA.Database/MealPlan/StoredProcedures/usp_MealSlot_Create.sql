CREATE PROCEDURE [MealPlan].[usp_MealSlot_Create]
    @MealPlanID INT,
    @DayOfWeek TINYINT,
    @MealType TINYINT,
    @RecipeID INT = NULL,
    @Servings DECIMAL(10,2) = 1,
    @ReplacementNote NVARCHAR(500) = NULL,
    @UserID INT,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [MealPlan].[MealSlot] (MealPlanID, DayOfWeek, MealType, RecipeID, Servings, ReplacementNote, CreatedBy, CreateDate)
    SELECT @MealPlanID, @DayOfWeek, @MealType, @RecipeID, ISNULL(NULLIF(@Servings, 0), 1), @ReplacementNote, @CreatedBy, @CreateDate
    FROM [MealPlan].[MealPlan] mp
    WHERE mp.MealPlanID = @MealPlanID AND mp.UserID = @UserID;

    SELECT ISNULL(CAST(SCOPE_IDENTITY() AS INT), 0);
END
