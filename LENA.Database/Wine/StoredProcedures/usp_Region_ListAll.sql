CREATE PROCEDURE [Wine].[usp_Region_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Region] ORDER BY RegionName;
END