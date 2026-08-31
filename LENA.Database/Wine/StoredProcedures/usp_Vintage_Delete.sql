CREATE PROCEDURE [Wine].[usp_Vintage_Delete]
    @VintageID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Wine].[Vintage] WHERE VintageID = @VintageID;

    SELECT @@ROWCOUNT;
END
