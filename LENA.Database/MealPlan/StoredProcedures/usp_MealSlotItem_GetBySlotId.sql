CREATE PROCEDURE [MealPlan].[usp_MealSlotItem_GetBySlotId]
    @MealSlotID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MealSlotItemID, MealSlotID, ItemID, Quantity, UnitOfMeasure, IsFromRecipe, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[MealSlotItem]
    WHERE MealSlotID = @MealSlotID;
END
