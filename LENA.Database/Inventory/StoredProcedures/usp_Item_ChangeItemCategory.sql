CREATE PROCEDURE [Inventory].[usp_Item_ChangeItemCategory]
    @ItemID INT,
    @CategoryID INT
AS
BEGIN
    UPDATE [Inventory].[Item] SET [CategoryID] = @CategoryID WHERE [ItemID] = @ItemID;
END