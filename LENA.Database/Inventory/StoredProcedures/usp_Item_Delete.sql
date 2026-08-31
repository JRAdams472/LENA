CREATE PROCEDURE [Inventory].[usp_Item_Delete]
    @ItemID INT
AS
BEGIN
    DELETE FROM [Inventory].[Item] WHERE [ItemID] = @ItemID;
END