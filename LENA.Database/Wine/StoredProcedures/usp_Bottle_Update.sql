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