CREATE TABLE [MealPlan].[MealSlotItem] (
    [MealSlotItemID] INT IDENTITY(1,1) NOT NULL,
    [MealSlotID] INT NOT NULL,
    [ItemID] INT NOT NULL,
    [Quantity] DECIMAL(10,2) NOT NULL,
    [UnitOfMeasure] NVARCHAR(20) NULL,
    [IsFromRecipe] BIT DEFAULT 0 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_MealSlotItem] PRIMARY KEY CLUSTERED ([MealSlotItemID] ASC),
    CONSTRAINT [FK_MealSlotItem_MealSlot] FOREIGN KEY ([MealSlotID]) REFERENCES [MealPlan].[MealSlot] ([MealSlotID]) ON DELETE CASCADE,
    CONSTRAINT [FK_MealSlotItem_Item] FOREIGN KEY ([ItemID]) REFERENCES [Inventory].[Item] ([ItemID])
);
GO
