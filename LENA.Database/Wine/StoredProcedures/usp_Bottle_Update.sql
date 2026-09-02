CREATE PROCEDURE [Wine].[usp_Bottle_Update]
    @UserID INT,
    @BottleID INT,
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
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET XACT_ABORT ON;
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    UPDATE [Wine].[Bottle]
    SET TypeID = @TypeID, CountryID = @CountryID, RegionID = @RegionID,
        VintageYear = @VintageYear, Vineyard = @Vineyard, ABV = @ABV,
        Acidity = @Acidity, TanninLevel = @TanninLevel, Body = @Body, Sweetness = @Sweetness, OakIntegration = @OakIntegration,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE BottleID = @BottleID;

    IF @@ROWCOUNT = 0
    BEGIN
        ROLLBACK TRANSACTION;
        SELECT 0;
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM [Wine].[UserBottle] WHERE UserID = @UserID AND BottleID = @BottleID)
    BEGIN
        UPDATE [Wine].[UserBottle]
        SET BottleNumber = @BottleNumber, BottleSize = @BottleSize, Quantity = @Quantity, PurchaseDate = @PurchaseDate,
            PurchasePrice = @PurchasePrice, StorageTemp = @StorageTemp, Location = @Location, Notes = @Notes, IsFavorite = @IsFavorite,
            LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
        WHERE UserID = @UserID AND BottleID = @BottleID;
    END
    ELSE
    BEGIN
        INSERT INTO [Wine].[UserBottle]
            (UserID, BottleID, BottleNumber, BottleSize, Quantity, PurchaseDate, PurchasePrice, StorageTemp, Location, Notes, IsFavorite,
             CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate)
        VALUES
            (@UserID, @BottleID, @BottleNumber, @BottleSize, @Quantity, @PurchaseDate, @PurchasePrice, @StorageTemp, @Location, @Notes, @IsFavorite,
             @LastUpdatedBy, ISNULL(@LastUpdatedDate, SYSUTCDATETIME()), @LastUpdatedBy, @LastUpdatedDate);
    END

    COMMIT TRANSACTION;

    SELECT @@ROWCOUNT;
END
