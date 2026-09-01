CREATE PROCEDURE [Wine].[usp_Region_ListAllPaged]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT * FROM [Wine].[Region] ORDER BY RegionName
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Wine].[Region];
END