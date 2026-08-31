CREATE PROCEDURE [Wine].[usp_Region_Delete]
    @RegionID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Wine].[Region] WHERE RegionID = @RegionID;

    SELECT @@ROWCOUNT;
END
