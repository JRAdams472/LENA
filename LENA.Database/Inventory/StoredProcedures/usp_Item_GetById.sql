CREATE PROCEDURE [Inventory].[usp_Item_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Inventory].[Item] WHERE [ItemID] = @Id;
END