SET XACT_ABORT ON;
SET NOCOUNT ON;

-- Create per-user bottle holding table
IF OBJECT_ID(N'[Wine].[UserBottle]', N'U') IS NULL
BEGIN
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

    CREATE INDEX [IX_UserBottle_BottleID] ON [Wine].[UserBottle] ([BottleID]);
END

DECLARE @DefaultUserID INT;
SELECT @DefaultUserID = [UserID] FROM [Identity].[User] WHERE [Provider] = 'google' AND [ExternalSubject] = 'legacy-default';

-- Backfill existing per-user bottle data for the legacy default user
IF @DefaultUserID IS NOT NULL
   AND COL_LENGTH(N'[Wine].[Bottle]', N'Quantity') IS NOT NULL
BEGIN
    INSERT INTO [Wine].[UserBottle] ([UserID], [BottleID], [BottleNumber], [BottleSize], [Quantity], [PurchaseDate], [PurchasePrice], [StorageTemp], [Location], [Notes], [IsFavorite], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate])
    SELECT @DefaultUserID, [BottleID], [BottleNumber], [BottleSize], [Quantity], [PurchaseDate], [PurchasePrice], [StorageTemp], [Location], [Notes], [IsFavorite], [CreatedBy], [CreateDate], [LastUpdatedBy], [LastUpdatedDate]
    FROM [Wine].[Bottle]
    WHERE NOT EXISTS (
        SELECT 1 FROM [Wine].[UserBottle]
        WHERE [UserID] = @DefaultUserID AND [BottleID] = [Wine].[Bottle].[BottleID]
    );
END

-- Drop the per-user columns from the catalog Bottle table
IF COL_LENGTH(N'[Wine].[Bottle]', N'IsFavorite') IS NOT NULL
BEGIN
    DECLARE @IsFavoriteConstraint NVARCHAR(255) = (
        SELECT dc.[name]
        FROM sys.default_constraints dc
        INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
        WHERE OBJECT_NAME(dc.parent_object_id) = 'Bottle'
          AND OBJECT_SCHEMA_NAME(dc.parent_object_id) = 'Wine'
          AND c.name = 'IsFavorite'
    );

    IF @IsFavoriteConstraint IS NOT NULL
        EXEC('ALTER TABLE [Wine].[Bottle] DROP CONSTRAINT [' + @IsFavoriteConstraint + ']');

    ALTER TABLE [Wine].[Bottle] DROP COLUMN [IsFavorite];
END

IF COL_LENGTH(N'[Wine].[Bottle]', N'BottleSize') IS NOT NULL
BEGIN
    DECLARE @BottleSizeConstraint NVARCHAR(255) = (
        SELECT dc.[name]
        FROM sys.default_constraints dc
        INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
        WHERE OBJECT_NAME(dc.parent_object_id) = 'Bottle'
          AND OBJECT_SCHEMA_NAME(dc.parent_object_id) = 'Wine'
          AND c.name = 'BottleSize'
    );

    IF @BottleSizeConstraint IS NOT NULL
        EXEC('ALTER TABLE [Wine].[Bottle] DROP CONSTRAINT [' + @BottleSizeConstraint + ']');

    ALTER TABLE [Wine].[Bottle] DROP COLUMN [BottleSize];
END

IF COL_LENGTH(N'[Wine].[Bottle]', N'Quantity') IS NOT NULL
BEGIN
    DECLARE @QuantityConstraint NVARCHAR(255) = (
        SELECT dc.[name]
        FROM sys.default_constraints dc
        INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
        WHERE OBJECT_NAME(dc.parent_object_id) = 'Bottle'
          AND OBJECT_SCHEMA_NAME(dc.parent_object_id) = 'Wine'
          AND c.name = 'Quantity'
    );

    IF @QuantityConstraint IS NOT NULL
        EXEC('ALTER TABLE [Wine].[Bottle] DROP CONSTRAINT [' + @QuantityConstraint + ']');

    ALTER TABLE [Wine].[Bottle] DROP COLUMN [Quantity];
END

IF COL_LENGTH(N'[Wine].[Bottle]', N'BottleNumber') IS NOT NULL
    ALTER TABLE [Wine].[Bottle] DROP COLUMN [BottleNumber];

IF COL_LENGTH(N'[Wine].[Bottle]', N'PurchaseDate') IS NOT NULL
    ALTER TABLE [Wine].[Bottle] DROP COLUMN [PurchaseDate];

IF COL_LENGTH(N'[Wine].[Bottle]', N'PurchasePrice') IS NOT NULL
    ALTER TABLE [Wine].[Bottle] DROP COLUMN [PurchasePrice];

IF COL_LENGTH(N'[Wine].[Bottle]', N'StorageTemp') IS NOT NULL
    ALTER TABLE [Wine].[Bottle] DROP COLUMN [StorageTemp];

IF COL_LENGTH(N'[Wine].[Bottle]', N'Location') IS NOT NULL
    ALTER TABLE [Wine].[Bottle] DROP COLUMN [Location];

IF COL_LENGTH(N'[Wine].[Bottle]', N'Notes') IS NOT NULL
    ALTER TABLE [Wine].[Bottle] DROP COLUMN [Notes];

-- Drop legacy indexes that referenced the removed per-user columns
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Bottle_CountryID_BottleNumber' AND OBJECT_NAME(object_id) = 'Bottle' AND OBJECT_SCHEMA_NAME(object_id) = 'Wine')
    DROP INDEX [IX_Bottle_CountryID_BottleNumber] ON [Wine].[Bottle];

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Bottle_RegionID_BottleNumber' AND OBJECT_NAME(object_id) = 'Bottle' AND OBJECT_SCHEMA_NAME(object_id) = 'Wine')
    DROP INDEX [IX_Bottle_RegionID_BottleNumber] ON [Wine].[Bottle];

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Bottle_TypeID_BottleNumber' AND OBJECT_NAME(object_id) = 'Bottle' AND OBJECT_SCHEMA_NAME(object_id) = 'Wine')
    DROP INDEX [IX_Bottle_TypeID_BottleNumber] ON [Wine].[Bottle];

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Bottle_VintageYear_BottleNumber' AND OBJECT_NAME(object_id) = 'Bottle' AND OBJECT_SCHEMA_NAME(object_id) = 'Wine')
    DROP INDEX [IX_Bottle_VintageYear_BottleNumber] ON [Wine].[Bottle];

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Bottle_IsFavorite_BottleNumber' AND OBJECT_NAME(object_id) = 'Bottle' AND OBJECT_SCHEMA_NAME(object_id) = 'Wine')
    DROP INDEX [IX_Bottle_IsFavorite_BottleNumber] ON [Wine].[Bottle];
