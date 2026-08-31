CREATE PROCEDURE [Wine].[usp_Bottle_GetAllByVintageYear]
    @VintageYear INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE VintageYear = @VintageYear ORDER BY BottleNumber;
END