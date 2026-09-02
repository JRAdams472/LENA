CREATE PROCEDURE [MealPlan].[usp_MealSlotItem_Delete]
    @MealSlotItemID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE msi
    FROM [MealPlan].[MealSlotItem] msi
    INNER JOIN [MealPlan].[MealSlot] s ON msi.MealSlotID = s.MealSlotID
    INNER JOIN [MealPlan].[MealPlan] mp ON s.MealPlanID = mp.MealPlanID
    WHERE msi.MealSlotItemID = @MealSlotItemID AND mp.UserID = @UserID;

    SELECT @@ROWCOUNT;
END
