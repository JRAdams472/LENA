CREATE PROCEDURE [Inventory].[usp_Item_AdjustQuantity]
    @ItemID INT,
    @Quantity DECIMAL(10, 2),
    @PurchaseDate DATETIME2 = NULL,
    @LastUpdatedBy NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @PurchaseDate IS NOT NULL
        UPDATE [Inventory].[Item]
        SET [CurrentQuantity] = @Quantity,
            [PurchaseDate] = @PurchaseDate,
            [LastUpdatedBy] = @LastUpdatedBy,
            [LastUpdatedDate] = SYSUTCDATETIME()
        WHERE [ItemID] = @ItemID;
    ELSE
        UPDATE [Inventory].[Item]
        SET [CurrentQuantity] = @Quantity,
            [LastUpdatedBy] = @LastUpdatedBy,
            [LastUpdatedDate] = SYSUTCDATETIME()
        WHERE [ItemID] = @ItemID;
END
