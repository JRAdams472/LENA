CREATE PROCEDURE [Wine].[usp_Vintage_Delete]
    @VintageID INT
AS
BEGIN
    DELETE FROM [Wine].[Vintage] WHERE VintageID = @VintageID;
END