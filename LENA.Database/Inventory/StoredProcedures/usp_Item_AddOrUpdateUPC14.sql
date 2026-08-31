CREATE PROCEDURE [Inventory].[usp_Item_AddOrUpdateUPC14]
    @ItemID INT,
    @UPC14 NVARCHAR(14) = NULL
AS
BEGIN
    UPDATE [Inventory].[Item] SET [UPC14] = @UPC14 WHERE [ItemID] = @ItemID;
END