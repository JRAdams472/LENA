CREATE PROCEDURE [Inventory].[usp_Item_AddOrUpdateUPC12]
    @ItemID INT,
    @UPC12 NVARCHAR(12) = NULL
AS
BEGIN
    UPDATE [Inventory].[Item] SET [UPC12] = @UPC12 WHERE [ItemID] = @ItemID;
END