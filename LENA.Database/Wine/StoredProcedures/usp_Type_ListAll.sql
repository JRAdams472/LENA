CREATE PROCEDURE [Wine].[usp_Type_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Type] ORDER BY TypeName;
END