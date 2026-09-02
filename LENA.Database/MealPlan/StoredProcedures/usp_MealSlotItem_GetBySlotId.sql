CREATE PROCEDURE [MealPlan].[usp_MealSlotItem_GetBySlotId]
    @MealSlotID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT msi.MealSlotItemID, msi.MealSlotID, msi.ItemID, msi.Quantity, msi.UnitOfMeasure, msi.IsFromRecipe, msi.CreatedBy, msi.CreateDate, msi.LastUpdatedBy, msi.LastUpdatedDate
    FROM [MealPlan].[MealSlotItem] msi
    INNER JOIN [MealPlan].[MealSlot] s ON msi.MealSlotID = s.MealSlotID
    INNER JOIN [MealPlan].[MealPlan] mp ON s.MealPlanID = mp.MealPlanID
    WHERE msi.MealSlotID = @MealSlotID AND mp.UserID = @UserID;
END
