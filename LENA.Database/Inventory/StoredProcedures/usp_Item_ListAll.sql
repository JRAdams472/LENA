CREATE PROCEDURE [Inventory].[usp_Item_ListAll]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT * FROM [Inventory].[Item] ORDER BY [Name]
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Inventory].[Item];
END