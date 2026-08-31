CREATE PROCEDURE [MealPlan].[usp_MealSlot_Delete]
    @MealSlotID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [MealPlan].[MealSlot]
    WHERE MealSlotID = @MealSlotID;

    SELECT @@ROWCOUNT;
END
