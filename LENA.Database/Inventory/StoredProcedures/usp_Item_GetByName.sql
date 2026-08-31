CREATE PROCEDURE [Inventory].[usp_Item_GetByName]
    @Name NVARCHAR(200)
AS
BEGIN
    SELECT * FROM [Inventory].[Item] WHERE [Name] = @Name;
END