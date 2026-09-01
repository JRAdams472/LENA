CREATE PROCEDURE [Wine].[usp_Bottle_ListAll]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT * FROM [Wine].[Bottle] ORDER BY BottleNumber
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Wine].[Bottle];
END