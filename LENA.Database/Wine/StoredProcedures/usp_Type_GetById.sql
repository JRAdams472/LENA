CREATE PROCEDURE [Wine].[usp_Type_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Type] WHERE TypeID = @Id;
END