CREATE PROCEDURE [Inventory].[usp_Item_AdjustQuantity]
    @ItemID INT,
    @Quantity DECIMAL(10, 2),
    @PurchaseDate DATETIME2 = NULL
AS
BEGIN
    IF @PurchaseDate IS NOT NULL
        UPDATE [Inventory].[Item] SET [CurrentQuantity] = @Quantity, [PurchaseDate] = @PurchaseDate WHERE [ItemID] = @ItemID;
    ELSE
        UPDATE [Inventory].[Item] SET [CurrentQuantity] = @Quantity WHERE [ItemID] = @ItemID;
END