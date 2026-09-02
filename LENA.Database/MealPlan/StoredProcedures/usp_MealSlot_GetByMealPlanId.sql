CREATE PROCEDURE [MealPlan].[usp_MealSlot_GetByMealPlanId]
    @MealPlanID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT s.MealSlotID, s.MealPlanID, s.DayOfWeek, s.MealType, s.RecipeID, s.Servings, s.ReplacementNote, s.CreatedBy, s.CreateDate, s.LastUpdatedBy, s.LastUpdatedDate
    FROM [MealPlan].[MealSlot] s
    INNER JOIN [MealPlan].[MealPlan] mp ON s.MealPlanID = mp.MealPlanID
    WHERE s.MealPlanID = @MealPlanID AND mp.UserID = @UserID
    ORDER BY s.DayOfWeek, s.MealType;
END
