CREATE TABLE [Wine].[UserBottle] (
    [UserBottleID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,
    [BottleID] INT NOT NULL,
    [BottleNumber] INT NULL,
    [BottleSize] NVARCHAR(20) NOT NULL CONSTRAINT [DF_UserBottle_BottleSize] DEFAULT '750ml',
    [Quantity] INT NOT NULL CONSTRAINT [DF_UserBottle_Quantity] DEFAULT 1,
    [PurchaseDate] DATETIME2 NULL,
    [PurchasePrice] DECIMAL(10,2) NULL,
    [StorageTemp] DECIMAL(5,1) NULL,
    [Location] NVARCHAR(100) NULL,
    [Notes] NVARCHAR(500) NULL,
    [IsFavorite] BIT NOT NULL CONSTRAINT [DF_UserBottle_IsFavorite] DEFAULT 0,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_UserBottle] PRIMARY KEY CLUSTERED ([UserBottleID]),
    CONSTRAINT [UQ_UserBottle_UserID_BottleID] UNIQUE ([UserID], [BottleID]),
    CONSTRAINT [FK_UserBottle_User] FOREIGN KEY ([UserID]) REFERENCES [Identity].[User] ([UserID]),
    CONSTRAINT [FK_UserBottle_Bottle] FOREIGN KEY ([BottleID]) REFERENCES [Wine].[Bottle] ([BottleID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_UserBottle_BottleID] ON [Wine].[UserBottle] ([BottleID]);
GO

CREATE INDEX [IX_UserBottle_UserID_BottleNumber] ON [Wine].[UserBottle] ([UserID] ASC, [BottleNumber] ASC);
GO
