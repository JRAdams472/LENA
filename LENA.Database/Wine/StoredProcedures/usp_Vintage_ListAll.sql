CREATE PROCEDURE [Wine].[usp_Vintage_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Vintage] ORDER BY Year;
END