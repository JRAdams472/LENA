CREATE PROCEDURE [MealPlan].[usp_MealSlotItem_Create]
    @MealSlotID INT,
    @ItemID INT,
    @Quantity DECIMAL(10,2),
    @UnitOfMeasure NVARCHAR(20) = NULL,
    @IsFromRecipe BIT = 0,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [MealPlan].[MealSlotItem] (MealSlotID, ItemID, Quantity, UnitOfMeasure, IsFromRecipe, CreatedBy, CreateDate)
    VALUES (@MealSlotID, @ItemID, @Quantity, @UnitOfMeasure, @IsFromRecipe, @CreatedBy, @CreateDate);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
