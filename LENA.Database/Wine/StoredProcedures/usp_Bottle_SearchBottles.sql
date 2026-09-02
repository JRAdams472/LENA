CREATE PROCEDURE [Wine].[usp_Bottle_SearchBottles]
    @UserID INT,
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @EscapedTerm NVARCHAR(130);
    SET @EscapedTerm = REPLACE(
                            REPLACE(
                                REPLACE(
                                    REPLACE(
                                        REPLACE(@SearchTerm, '|', '||'),
                                        '%', '|%'),
                                    '_', '|_'),
                                '[', '|['),
                            ']', '|]');
    DECLARE @Pattern NVARCHAR(150) = '%' + @EscapedTerm + '%';
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
    WHERE (ub.BottleNumber IS NOT NULL AND CAST(ub.BottleNumber AS NVARCHAR(10)) LIKE @Pattern ESCAPE '|')
       OR (b.Vineyard LIKE @Pattern ESCAPE '|')
       OR (ub.Notes LIKE @Pattern ESCAPE '|')
    ORDER BY ub.BottleNumber;
END