CREATE PROCEDURE [MealPlan].[usp_MealSlot_Update]
    @MealSlotID INT,
    @MealPlanID INT,
    @DayOfWeek TINYINT,
    @MealType TINYINT,
    @RecipeID INT = NULL,
    @Servings DECIMAL(10,2) = 1,
    @ReplacementNote NVARCHAR(500) = NULL,
    @UserID INT,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE s
    SET MealPlanID = @MealPlanID,
        DayOfWeek = @DayOfWeek,
        MealType = @MealType,
        RecipeID = @RecipeID,
        Servings = ISNULL(NULLIF(@Servings, 0), 1),
        ReplacementNote = @ReplacementNote,
        LastUpdatedBy = @LastUpdatedBy,
        LastUpdatedDate = @LastUpdatedDate
    FROM [MealPlan].[MealSlot] s
    INNER JOIN [MealPlan].[MealPlan] currentPlan ON s.MealPlanID = currentPlan.MealPlanID
    INNER JOIN [MealPlan].[MealPlan] targetPlan ON targetPlan.MealPlanID = @MealPlanID
    WHERE s.MealSlotID = @MealSlotID
      AND currentPlan.UserID = @UserID
      AND targetPlan.UserID = @UserID;

    SELECT @@ROWCOUNT;
END
