CREATE PROCEDURE [Inventory].[usp_Item_ListAll]
AS
BEGIN
    SELECT * FROM [Inventory].[Item] ORDER BY [Name];
END