CREATE PROCEDURE [Wine].[usp_Bottle_GetTotalBottleCount]
AS
BEGIN
    SELECT COUNT(*) FROM [Wine].[Bottle];
END