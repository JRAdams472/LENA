CREATE PROCEDURE [Wine].[usp_Bottle_GetFavorites]
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE IsFavorite = 1 ORDER BY BottleNumber;
END