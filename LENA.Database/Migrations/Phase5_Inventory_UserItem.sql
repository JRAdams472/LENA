SET XACT_ABORT ON;
SET NOCOUNT ON;

-- Create per-user item holding table
IF OBJECT_ID(N'[Inventory].[UserItem]', N'U') IS NULL
BEGIN
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
        CONSTRAINT [PK_UserItem] PRIMARY KEY CLUSTERED ([UserItemID]),
        CONSTRAINT [UQ_UserItem_UserID_ItemID] UNIQUE ([UserID], [ItemID]),
        CONSTRAINT [FK_UserItem_User] FOREIGN KEY ([UserID]) REFERENCES [Identity].[User] ([UserID]),
        CONSTRAINT [FK_UserItem_Item] FOREIGN KEY ([ItemID]) REFERENCES [Inventory].[Item] ([ItemID]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_UserItem_ItemID] ON [Inventory].[UserItem] ([ItemID]);
END
GO

IF OBJECT_ID('tempdb..#DefaultUserID') IS NOT NULL DROP TABLE #DefaultUserID;
GO

SELECT [UserID] INTO #DefaultUserID FROM [Identity].[User] WHERE [Provider] = 'google' AND [ExternalSubject] = 'legacy-default';
GO

-- Backfill existing per-user item data for the legacy default user
IF (SELECT COUNT(*) FROM #DefaultUserID) > 0
   AND COL_LENGTH(N'[Inventory].[Item]', N'CurrentQuantity') IS NOT NULL
BEGIN
    EXEC('
    INSERT INTO [Inventory].[UserItem] ([UserID], [ItemID], [CurrentQuantity], [MinQuantity], [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate])
    SELECT (SELECT [UserID] FROM #DefaultUserID), [ItemID], [CurrentQuantity], [MinQuantity], [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]
    FROM [Inventory].[Item]
    WHERE NOT EXISTS (
        SELECT 1 FROM [Inventory].[UserItem]
        WHERE [UserID] = (SELECT [UserID] FROM #DefaultUserID) AND [ItemID] = [Inventory].[Item].[ItemID]
    );
    ');
END
GO

-- Drop the per-user columns from the catalog Item table
IF COL_LENGTH(N'[Inventory].[Item]', N'IsFavorite') IS NOT NULL
BEGIN
    DECLARE @IsFavoriteConstraint NVARCHAR(255) = (
        SELECT dc.[name]
        FROM sys.default_constraints dc
        INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
        WHERE OBJECT_NAME(dc.parent_object_id) = 'Item'
          AND OBJECT_SCHEMA_NAME(dc.parent_object_id) = 'Inventory'
          AND c.name = 'IsFavorite'
    );

    IF @IsFavoriteConstraint IS NOT NULL
        EXEC('ALTER TABLE [Inventory].[Item] DROP CONSTRAINT [' + @IsFavoriteConstraint + ']');

    EXEC('ALTER TABLE [Inventory].[Item] DROP COLUMN [IsFavorite]');
END
GO

IF COL_LENGTH(N'[Inventory].[Item]', N'CurrentQuantity') IS NOT NULL
    EXEC('ALTER TABLE [Inventory].[Item] DROP COLUMN [CurrentQuantity]');
GO

IF COL_LENGTH(N'[Inventory].[Item]', N'MinQuantity') IS NOT NULL
    EXEC('ALTER TABLE [Inventory].[Item] DROP COLUMN [MinQuantity]');
GO

IF COL_LENGTH(N'[Inventory].[Item]', N'PurchaseDate') IS NOT NULL
    EXEC('ALTER TABLE [Inventory].[Item] DROP COLUMN [PurchaseDate]');
GO

IF COL_LENGTH(N'[Inventory].[Item]', N'ExpiryDate') IS NOT NULL
    EXEC('ALTER TABLE [Inventory].[Item] DROP COLUMN [ExpiryDate]');
GO

IF COL_LENGTH(N'[Inventory].[Item]', N'Notes') IS NOT NULL
    EXEC('ALTER TABLE [Inventory].[Item] DROP COLUMN [Notes]');
GO

-- Drop the legacy InStock table (superseded by UserItem)
IF OBJECT_ID(N'[Inventory].[InStock]', N'U') IS NOT NULL
BEGIN
    DECLARE @InStockFkName NVARCHAR(255) = (
        SELECT fk.name
        FROM sys.foreign_keys fk
        INNER JOIN sys.tables t ON fk.parent_object_id = t.object_id
        INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
        WHERE s.name = 'Inventory' AND t.name = 'InStock'
    );

    IF @InStockFkName IS NOT NULL
        EXEC('ALTER TABLE [Inventory].[InStock] DROP CONSTRAINT [' + @InStockFkName + ']');

    DROP TABLE [Inventory].[InStock];
END
