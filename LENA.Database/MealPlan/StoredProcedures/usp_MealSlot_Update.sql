CREATE PROCEDURE [MealPlan].[usp_MealSlot_Update]
    @MealSlotID INT,
    @MealPlanID INT,
    @DayOfWeek TINYINT,
    @MealType TINYINT,
    @RecipeID INT = NULL,
    @ReplacementNote NVARCHAR(500) = NULL,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [MealPlan].[MealSlot]
    SET MealPlanID = @MealPlanID,
        DayOfWeek = @DayOfWeek,
        MealType = @MealType,
        RecipeID = @RecipeID,
        ReplacementNote = @ReplacementNote,
        LastUpdatedBy = @LastUpdatedBy,
        LastUpdatedDate = @LastUpdatedDate
    WHERE MealSlotID = @MealSlotID;

    SELECT @@ROWCOUNT;
END
