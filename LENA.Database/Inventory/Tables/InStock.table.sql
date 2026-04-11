CREATE TABLE [Inventory].[InStock] (
    [StockID] INT IDENTITY(1,1) NOT NULL,
    [ItemID] INT NOT NULL,
    [QuantityOnHand] DECIMAL(10,2) NOT NULL,
    [LastUpdatedDate] DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    CONSTRAINT [PK_InStock] PRIMARY KEY CLUSTERED ([StockID] ASC),
    CONSTRAINT [FK_InStock_Item] FOREIGN KEY ([ItemID]) REFERENCES [Inventory].[Item] ([ItemID])
);
