CREATE PROCEDURE [Inventory].[usp_Item_ListAll]
AS
BEGIN
    SELECT * FROM [Inventory].[Item] ORDER BY [Name];
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Inventory].[Item] WHERE [ItemID] = @Id;
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_GetByName]
    @Name NVARCHAR(200)
AS
BEGIN
    SELECT * FROM [Inventory].[Item] WHERE [Name] = @Name;
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_Create]
    @Name NVARCHAR(200),
    @Brand NVARCHAR(100) = NULL,
    @UPC12 NVARCHAR(12) = NULL,
    @UPC14 NVARCHAR(14) = NULL,
    @CategoryID INT,
    @Unit NVARCHAR(20),
    @CurrentQuantity DECIMAL(10, 2),
    @MinQuantity DECIMAL(10, 2) = NULL,
    @PurchaseDate DATETIME2,
    @ExpiryDate DATETIME2 = NULL,
    @Notes NVARCHAR(500) = NULL,
    @IsFavorite BIT = 0,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Inventory].[Item]
        ([Name], [Brand], [UPC12], [UPC14], [CategoryID], [Unit], [CurrentQuantity], [MinQuantity],
         [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate])
    VALUES
        (@Name, @Brand, @UPC12, @UPC14, @CategoryID, @Unit, @CurrentQuantity, @MinQuantity,
         @PurchaseDate, @ExpiryDate, @Notes, @IsFavorite, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_Update]
    @ItemID INT,
    @Name NVARCHAR(200),
    @Brand NVARCHAR(100) = NULL,
    @UPC12 NVARCHAR(12) = NULL,
    @UPC14 NVARCHAR(14) = NULL,
    @CategoryID INT,
    @Unit NVARCHAR(20),
    @CurrentQuantity DECIMAL(10, 2),
    @MinQuantity DECIMAL(10, 2) = NULL,
    @PurchaseDate DATETIME2,
    @ExpiryDate DATETIME2 = NULL,
    @Notes NVARCHAR(500) = NULL,
    @IsFavorite BIT = 0,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    UPDATE [Inventory].[Item]
    SET [Name] = @Name, [Brand] = @Brand, [UPC12] = @UPC12, [UPC14] = @UPC14,
        [CategoryID] = @CategoryID, [Unit] = @Unit, [CurrentQuantity] = @CurrentQuantity,
        [MinQuantity] = @MinQuantity, [PurchaseDate] = @PurchaseDate, [ExpiryDate] = @ExpiryDate,
        [Notes] = @Notes, [IsFavorite] = @IsFavorite,
        [LastUpdatedBy] = @LastUpdatedBy, [LastUpdatedDate] = @LastUpdatedDate
    WHERE [ItemID] = @ItemID;
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_Delete]
    @ItemID INT
AS
BEGIN
    DELETE FROM [Inventory].[Item] WHERE [ItemID] = @ItemID;
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_ChangeItemCategory]
    @ItemID INT,
    @CategoryID INT
AS
BEGIN
    UPDATE [Inventory].[Item] SET [CategoryID] = @CategoryID WHERE [ItemID] = @ItemID;
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_AddOrUpdateUPC12]
    @ItemID INT,
    @UPC12 NVARCHAR(12) = NULL
AS
BEGIN
    UPDATE [Inventory].[Item] SET [UPC12] = @UPC12 WHERE [ItemID] = @ItemID;
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_AddOrUpdateUPC14]
    @ItemID INT,
    @UPC14 NVARCHAR(14) = NULL
AS
BEGIN
    UPDATE [Inventory].[Item] SET [UPC14] = @UPC14 WHERE [ItemID] = @ItemID;
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_AdjustQuantity]
    @ItemID INT,
    @Quantity DECIMAL(10, 2),
    @PurchaseDate DATETIME2 = NULL
AS
BEGIN
    IF @PurchaseDate IS NOT NULL
        UPDATE [Inventory].[Item] SET [CurrentQuantity] = @Quantity, [PurchaseDate] = @PurchaseDate WHERE [ItemID] = @ItemID;
    ELSE
        UPDATE [Inventory].[Item] SET [CurrentQuantity] = @Quantity WHERE [ItemID] = @ItemID;
END
GO

CREATE PROCEDURE [Inventory].[usp_Item_SetFavorite]
    @ItemID INT,
    @IsFavorite BIT
AS
BEGIN
    UPDATE [Inventory].[Item] SET [IsFavorite] = @IsFavorite WHERE [ItemID] = @ItemID;
END
GO
