CREATE PROCEDURE [Wine].[usp_Bottle_Create]
    @UserID INT,
    @BottleNumber INT = NULL,
    @TypeID INT,
    @CountryID INT,
    @RegionID INT,
    @VintageYear INT,
    @Vineyard NVARCHAR(200) = NULL,
    @ABV DECIMAL(5, 2) = NULL,
    @Acidity TINYINT = NULL,
    @TanninLevel TINYINT = NULL,
    @Body TINYINT = NULL,
    @Sweetness TINYINT = NULL,
    @OakIntegration BIT = NULL,
    @BottleSize NVARCHAR(20) = '750ml',
    @Quantity INT = 1,
    @PurchaseDate DATETIME2 = NULL,
    @PurchasePrice DECIMAL(10, 2) = NULL,
    @StorageTemp DECIMAL(5, 1) = NULL,
    @Location NVARCHAR(100) = NULL,
    @Notes NVARCHAR(500) = NULL,
    @IsFavorite BIT = 0,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET XACT_ABORT ON;
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    INSERT INTO [Wine].[Bottle]
        (TypeID, CountryID, RegionID, VintageYear, Vineyard, ABV, Acidity, TanninLevel, Body, Sweetness, OakIntegration,
         CreatedBy, CreateDate)
    VALUES
        (@TypeID, @CountryID, @RegionID, @VintageYear, @Vineyard, @ABV, @Acidity, @TanninLevel, @Body, @Sweetness, @OakIntegration,
         @CreatedBy, @CreateDate);

    DECLARE @BottleID INT = CAST(SCOPE_IDENTITY() as int);

    INSERT INTO [Wine].[UserBottle]
        (UserID, BottleID, BottleNumber, BottleSize, Quantity, PurchaseDate, PurchasePrice, StorageTemp, Location, Notes, IsFavorite,
         CreatedBy, CreateDate)
    VALUES
        (@UserID, @BottleID, @BottleNumber, @BottleSize, @Quantity, @PurchaseDate, @PurchasePrice, @StorageTemp, @Location, @Notes, @IsFavorite,
         @CreatedBy, @CreateDate);

    COMMIT TRANSACTION;

    SELECT @BottleID;
END