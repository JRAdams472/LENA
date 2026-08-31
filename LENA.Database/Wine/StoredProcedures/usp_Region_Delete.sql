CREATE PROCEDURE [Wine].[usp_Region_Delete]
    @RegionID INT
AS
BEGIN
    DELETE FROM [Wine].[Region] WHERE RegionID = @RegionID;
END