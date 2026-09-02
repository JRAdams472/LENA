CREATE PROCEDURE [Inventory].[usp_Item_SetFavorite]
    @ItemID INT,
    @UserID INT,
    @IsFavorite BIT,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2,
    @LastUpdatedBy NVARCHAR(100),
    @LastUpdatedDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    IF @IsFavorite = 1
    BEGIN
        MERGE [Inventory].[UserItem] AS target
        USING (VALUES (@UserID, @ItemID, @IsFavorite)) AS source (UserID, ItemID, IsFavorite)
        ON target.[UserID] = source.UserID AND target.[ItemID] = source.ItemID
        WHEN MATCHED THEN
            UPDATE SET [IsFavorite] = 1, [LastUpdatedBy] = @LastUpdatedBy, [LastUpdatedDate] = @LastUpdatedDate
        WHEN NOT MATCHED THEN
            INSERT ([UserID], [ItemID], [CurrentQuantity], [MinQuantity], [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate])
            VALUES (source.UserID, source.ItemID, 0, NULL, NULL, NULL, NULL, 1, @CreatedBy, @CreateDate);
    END
    ELSE
    BEGIN
        UPDATE [Inventory].[UserItem]
        SET [IsFavorite] = 0, [LastUpdatedBy] = @LastUpdatedBy, [LastUpdatedDate] = @LastUpdatedDate
        WHERE [UserID] = @UserID AND [ItemID] = @ItemID;
    END
END