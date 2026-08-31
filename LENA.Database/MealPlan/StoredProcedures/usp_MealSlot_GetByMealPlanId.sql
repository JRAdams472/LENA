CREATE PROCEDURE [MealPlan].[usp_MealSlot_GetByMealPlanId]
    @MealPlanID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MealSlotID, MealPlanID, DayOfWeek, MealType, RecipeID, ReplacementNote, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[MealSlot]
    WHERE MealPlanID = @MealPlanID
    ORDER BY DayOfWeek, MealType;
END
