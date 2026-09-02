CREATE PROCEDURE [Wine].[usp_Bottle_ListAllPaged]
    @UserID INT,
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

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
    ORDER BY ub.BottleNumber
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*)
    FROM [Wine].[Bottle] b
    INNER JOIN [Wine].[UserBottle] ub ON ub.BottleID = b.BottleID AND ub.UserID = @UserID;
END