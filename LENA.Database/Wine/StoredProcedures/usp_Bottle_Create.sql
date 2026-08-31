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