CREATE TABLE [MealPlan].[GroceryListItem] (
    [GroceryListItemID] INT IDENTITY(1,1) NOT NULL,
    [GroceryListID] INT NOT NULL,
    [ItemID] INT NULL,
    [ManualItemName] NVARCHAR(200) NULL,
    [QuantityNeeded] DECIMAL(10,2) NOT NULL,
    [UnitOfMeasure] NVARCHAR(20) NULL,
    [Source] NVARCHAR(50) NOT NULL,
    [IsChecked] BIT DEFAULT 0 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_GroceryListItem] PRIMARY KEY CLUSTERED ([GroceryListItemID] ASC),
    CONSTRAINT [FK_GroceryListItem_GroceryList] FOREIGN KEY ([GroceryListID]) REFERENCES [MealPlan].[GroceryList] ([GroceryListID]) ON DELETE CASCADE,
    CONSTRAINT [FK_GroceryListItem_Item] FOREIGN KEY ([ItemID]) REFERENCES [Inventory].[Item] ([ItemID]) ON DELETE SET NULL
);
GO
