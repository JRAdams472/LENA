CREATE TABLE [Recipe].[RecipeItem] (
    [RecipeID] INT NOT NULL,
    [ItemID] INT NOT NULL,
    [Quantity] DECIMAL(10,2) NOT NULL,
    [UnitOfMeasure] NVARCHAR(20) NULL,
    [Notes] NVARCHAR(500) NULL,
    [IsOptional] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_RecipeItem] PRIMARY KEY CLUSTERED ([RecipeID] ASC, [ItemID] ASC),
    CONSTRAINT [FK_RecipeItem_Recipe] FOREIGN KEY ([RecipeID]) REFERENCES [Recipe].[Recipe] ([RecipeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_RecipeItem_Item] FOREIGN KEY ([ItemID]) REFERENCES [Inventory].[Item] ([ItemID])
);
GO
