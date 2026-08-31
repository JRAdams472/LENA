CREATE PROCEDURE [MealPlan].[usp_MealSlotItem_Delete]
    @MealSlotItemID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [MealPlan].[MealSlotItem]
    WHERE MealSlotItemID = @MealSlotItemID;

    SELECT @@ROWCOUNT;
END
