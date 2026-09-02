CREATE TABLE [Inventory].[UserItem] (
    [UserItemID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,
    [ItemID] INT NOT NULL,
    [CurrentQuantity] DECIMAL(10,2) NOT NULL CONSTRAINT [DF_UserItem_CurrentQuantity] DEFAULT 0,
    [MinQuantity] DECIMAL(10,2) NULL,
    [PurchaseDate] DATETIME2 NULL,
    [ExpiryDate] DATETIME2 NULL,
    [Notes] NVARCHAR(500) NULL,
    [IsFavorite] BIT NOT NULL CONSTRAINT [DF_UserItem_IsFavorite] DEFAULT 0,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_UserItem] PRIMARY KEY CLUSTERED ([UserItemID] ASC),
    CONSTRAINT [UQ_UserItem_UserID_ItemID] UNIQUE ([UserID], [ItemID]),
    CONSTRAINT [FK_UserItem_User] FOREIGN KEY ([UserID]) REFERENCES [Identity].[User] ([UserID]),
    CONSTRAINT [FK_UserItem_Item] FOREIGN KEY ([ItemID]) REFERENCES [Inventory].[Item] ([ItemID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_UserItem_ItemID] ON [Inventory].[UserItem] ([ItemID]);
GO
