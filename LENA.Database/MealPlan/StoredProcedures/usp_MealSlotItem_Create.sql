CREATE PROCEDURE [MealPlan].[usp_MealSlotItem_Create]
    @MealSlotID INT,
    @ItemID INT,
    @Quantity DECIMAL(10,2),
    @UnitOfMeasure NVARCHAR(20) = NULL,
    @IsFromRecipe BIT = 0,
    @UserID INT,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [MealPlan].[MealSlotItem] (MealSlotID, ItemID, Quantity, UnitOfMeasure, IsFromRecipe, CreatedBy, CreateDate)
    SELECT @MealSlotID, @ItemID, @Quantity, @UnitOfMeasure, @IsFromRecipe, @CreatedBy, @CreateDate
    FROM [MealPlan].[MealSlot] s
    INNER JOIN [MealPlan].[MealPlan] mp ON s.MealPlanID = mp.MealPlanID
    WHERE s.MealSlotID = @MealSlotID AND mp.UserID = @UserID;

    SELECT ISNULL(CAST(SCOPE_IDENTITY() AS INT), 0);
END
