CREATE PROCEDURE [MealPlan].[usp_GroceryListItem_Create]
    @GroceryListID INT,
    @ItemID INT = NULL,
    @ManualItemName NVARCHAR(200) = NULL,
    @QuantityNeeded DECIMAL(10,2),
    @UnitOfMeasure NVARCHAR(20) = NULL,
    @Source NVARCHAR(50),
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [MealPlan].[GroceryListItem] (GroceryListID, ItemID, ManualItemName, QuantityNeeded, UnitOfMeasure, Source, IsChecked, CreatedBy, CreateDate)
    VALUES (@GroceryListID, @ItemID, @ManualItemName, @QuantityNeeded, @UnitOfMeasure, @Source, 0, @CreatedBy, @CreateDate);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
