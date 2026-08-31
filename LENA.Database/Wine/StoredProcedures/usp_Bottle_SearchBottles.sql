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