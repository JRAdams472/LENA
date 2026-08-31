CREATE PROCEDURE [Wine].[usp_Region_GetByName]
    @Name NVARCHAR(100)
AS
BEGIN
    SELECT * FROM [Wine].[Region] WHERE RegionName = @Name;
END