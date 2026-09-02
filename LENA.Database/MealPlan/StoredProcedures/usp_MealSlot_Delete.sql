CREATE PROCEDURE [MealPlan].[usp_MealSlot_Delete]
    @MealSlotID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE s
    FROM [MealPlan].[MealSlot] s
    INNER JOIN [MealPlan].[MealPlan] mp ON s.MealPlanID = mp.MealPlanID
    WHERE s.MealSlotID = @MealSlotID AND mp.UserID = @UserID;

    SELECT @@ROWCOUNT;
END
