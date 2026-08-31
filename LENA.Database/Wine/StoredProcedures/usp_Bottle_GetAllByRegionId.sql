CREATE PROCEDURE [Wine].[usp_Bottle_GetAllByRegionId]
    @RegionId INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE RegionID = @RegionId ORDER BY BottleNumber;
END