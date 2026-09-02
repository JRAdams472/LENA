CREATE PROCEDURE [Wine].[usp_Bottle_GetFavorites]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT b.[BottleID], b.[TypeID], b.[CountryID], b.[RegionID], b.[VintageYear], b.[Vineyard], b.[ABV], b.[Acidity], b.[TanninLevel], b.[Body], b.[Sweetness], b.[OakIntegration],
           COALESCE(ub.[BottleNumber], b.[BottleID]) AS [BottleNumber],
           COALESCE(ub.[BottleSize], '750ml') AS [BottleSize],
           COALESCE(ub.[Quantity], 0) AS [Quantity],
           ub.[PurchaseDate],
           ub.[PurchasePrice],
           ub.[StorageTemp],
           ub.[Location],
           ub.[Notes],
           COALESCE(ub.[IsFavorite], 0) AS [IsFavorite],
           b.[CreatedBy], b.[CreateDate], b.[LastUpdatedBy], b.[LastUpdatedDate]
    FROM [Wine].[Bottle] b
    INNER JOIN [Wine].[UserBottle] ub ON ub.BottleID = b.BottleID AND ub.UserID = @UserID
    WHERE ub.IsFavorite = 1
    ORDER BY ub.BottleNumber;
END