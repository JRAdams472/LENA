CREATE PROCEDURE [Inventory].[usp_Item_AdjustQuantity]
    @ItemID INT,
    @UserID INT,
    @Quantity DECIMAL(10, 2),
    @PurchaseDate DATETIME2 = NULL,
    @LastUpdatedBy NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Now DATETIME2 = SYSUTCDATETIME();

    IF @PurchaseDate IS NOT NULL
    BEGIN
        IF EXISTS (SELECT 1 FROM [Inventory].[UserItem] WHERE [UserID] = @UserID AND [ItemID] = @ItemID)
            UPDATE [Inventory].[UserItem]
            SET [CurrentQuantity] = @Quantity,
                [PurchaseDate] = @PurchaseDate,
                [LastUpdatedBy] = @LastUpdatedBy,
                [LastUpdatedDate] = SYSUTCDATETIME()
            WHERE [UserID] = @UserID AND [ItemID] = @ItemID;
        ELSE
            INSERT INTO [Inventory].[UserItem] ([UserID], [ItemID], [CurrentQuantity], [MinQuantity], [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate])
            VALUES (@UserID, @ItemID, @Quantity, NULL, @PurchaseDate, NULL, NULL, 0, @LastUpdatedBy, @Now);
    END
    ELSE
    BEGIN
        IF EXISTS (SELECT 1 FROM [Inventory].[UserItem] WHERE [UserID] = @UserID AND [ItemID] = @ItemID)
            UPDATE [Inventory].[UserItem]
            SET [CurrentQuantity] = @Quantity,
                [LastUpdatedBy] = @LastUpdatedBy,
                [LastUpdatedDate] = SYSUTCDATETIME()
            WHERE [UserID] = @UserID AND [ItemID] = @ItemID;
        ELSE
            INSERT INTO [Inventory].[UserItem] ([UserID], [ItemID], [CurrentQuantity], [MinQuantity], [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate])
            VALUES (@UserID, @ItemID, @Quantity, NULL, NULL, NULL, NULL, 0, @LastUpdatedBy, @Now);
    END
END
