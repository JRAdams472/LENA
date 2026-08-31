CREATE PROCEDURE [Wine].[usp_Bottle_SearchBottles]
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @EscapedTerm NVARCHAR(130);
    SET @EscapedTerm = REPLACE(
                            REPLACE(
                                REPLACE(
                                    REPLACE(
                                        REPLACE(@SearchTerm, '|', '||'),
                                        '%', '|%'),
                                    '_', '|_'),
                                '[', '|['),
                            ']', '|]');
    DECLARE @Pattern NVARCHAR(150) = '%' + @EscapedTerm + '%';
    SELECT * FROM [Wine].[Bottle]
    WHERE (BottleNumber IS NOT NULL AND CAST(BottleNumber AS NVARCHAR(10)) LIKE @Pattern ESCAPE '|')
       OR (Vineyard LIKE @Pattern ESCAPE '|')
       OR (Notes LIKE @Pattern ESCAPE '|')
    ORDER BY BottleNumber;
END