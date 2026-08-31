CREATE PROCEDURE [Wine].[usp_Bottle_GetAllByCountryId]
    @CountryId INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE CountryID = @CountryId ORDER BY BottleNumber;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_GetAllByRegionId]
    @RegionId INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE RegionID = @RegionId ORDER BY BottleNumber;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_GetAllByTypeId]
    @TypeId INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE TypeID = @TypeId ORDER BY BottleNumber;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_GetAllByVintageYear]
    @VintageYear INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE VintageYear = @VintageYear ORDER BY BottleNumber;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_GetFavorites]
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE IsFavorite = 1 ORDER BY BottleNumber;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_SearchBottles]
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT * FROM [Wine].[Bottle]
    WHERE (BottleNumber IS NOT NULL AND CAST(BottleNumber AS NVARCHAR(10)) LIKE @SearchTerm)
       OR (Vineyard LIKE @SearchTerm)
       OR (Notes LIKE @SearchTerm)
    ORDER BY BottleNumber;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_GetTotalBottleCount]
AS
BEGIN
    SELECT COUNT(*) FROM [Wine].[Bottle];
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_Create]
    @BottleNumber INT = NULL,
    @TypeID INT,
    @CountryID INT,
    @RegionID INT,
    @VintageYear INT,
    @Vineyard NVARCHAR(200) = NULL,
    @ABV DECIMAL(5, 2) = NULL,
    @BottleSize NVARCHAR(20) = '750ml',
    @Quantity INT = 1,
    @PurchaseDate DATETIME2,
    @PurchasePrice DECIMAL(10, 2) = NULL,
    @StorageTemp DECIMAL(5, 1) = NULL,
    @Location NVARCHAR(100) = NULL,
    @Notes NVARCHAR(500) = NULL,
    @IsFavorite BIT = 0,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Wine].[Bottle]
        (BottleNumber, TypeID, CountryID, RegionID, VintageYear, Vineyard, ABV, BottleSize, Quantity,
         PurchaseDate, PurchasePrice, StorageTemp, Location, Notes, IsFavorite, CreatedBy, CreateDate)
    VALUES
        (@BottleNumber, @TypeID, @CountryID, @RegionID, @VintageYear, @Vineyard, @ABV, @BottleSize, @Quantity,
         @PurchaseDate, @PurchasePrice, @StorageTemp, @Location, @Notes, @IsFavorite, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE BottleID = @Id;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] ORDER BY BottleNumber;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_Update]
    @BottleID INT,
    @BottleNumber INT = NULL,
    @TypeID INT,
    @CountryID INT,
    @RegionID INT,
    @VintageYear INT,
    @Vineyard NVARCHAR(200) = NULL,
    @ABV DECIMAL(5, 2) = NULL,
    @BottleSize NVARCHAR(20) = '750ml',
    @Quantity INT = 1,
    @PurchaseDate DATETIME2,
    @PurchasePrice DECIMAL(10, 2) = NULL,
    @StorageTemp DECIMAL(5, 1) = NULL,
    @Location NVARCHAR(100) = NULL,
    @Notes NVARCHAR(500) = NULL,
    @IsFavorite BIT = 0,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    UPDATE [Wine].[Bottle]
    SET BottleNumber = @BottleNumber, TypeID = @TypeID, CountryID = @CountryID, RegionID = @RegionID,
        VintageYear = @VintageYear, Vineyard = @Vineyard, ABV = @ABV,
        BottleSize = @BottleSize, Quantity = @Quantity, PurchaseDate = @PurchaseDate,
        PurchasePrice = @PurchasePrice, StorageTemp = @StorageTemp, Location = @Location,
        Notes = @Notes, IsFavorite = @IsFavorite,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE BottleID = @BottleID;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_Delete]
    @BottleID INT
AS
BEGIN
    DELETE FROM [Wine].[Bottle] WHERE BottleID = @BottleID;
END
GO

CREATE PROCEDURE [Wine].[usp_Bottle_GetByName]
    @Name NVARCHAR(200)
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE Vineyard = @Name;
END
GO
